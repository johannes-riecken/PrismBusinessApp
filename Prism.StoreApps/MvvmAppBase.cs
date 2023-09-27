

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.Practices.Prism.StoreApps.Interfaces;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.ApplicationModel.Resources;
using Windows.ApplicationModel.Search;
using Windows.UI.ApplicationSettings;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Microsoft.Practices.Prism.StoreApps
{
    public abstract class MvvmAppBase : Application
    {
        protected MvvmAppBase()
        {
            this.Suspending += OnSuspending;
        }

        protected ISessionStateService SessionStateService { get; set; }

        protected INavigationService NavigationService { get; set; }

        public bool IsSuspending { get; private set; }

        protected abstract void OnLaunchApplication(LaunchActivatedEventArgs args);
        
        protected virtual void OnSearchApplication(SearchQueryArguments args) { }

        protected virtual Type GetPageType(string pageToken)
        {
            var assemblyQualifiedAppType = this.GetType().GetTypeInfo().AssemblyQualifiedName;

            var pageNameWithParameter = assemblyQualifiedAppType.Replace(this.GetType().FullName, this.GetType().Namespace + ".Views.{0}Page");

            var viewFullName = string.Format(CultureInfo.InvariantCulture, pageNameWithParameter, pageToken);
            var viewType = Type.GetType(viewFullName);

            if (viewType == null)
            {
                var resourceLoader = ResourceLoader.GetForCurrentView(Constants.StoreAppsInfrastructureResourceMapId);
                throw new ArgumentException(
                    string.Format(CultureInfo.InvariantCulture, resourceLoader.GetString("DefaultPageTypeLookupErrorMessage"), pageToken, this.GetType().Namespace + ".Views"),
                    "pageToken");
            }

            return viewType;
        }

        protected virtual void OnRegisterKnownTypesForSerialization() { }

        protected virtual void OnInitialize(IActivatedEventArgs args) { }

        protected virtual IList<SettingsCommand> GetSettingsCommands()
        {
            return null;
        }

        protected virtual object Resolve(Type type)
        {
            return Activator.CreateInstance(type);
        }

        protected override async void OnLaunched(LaunchActivatedEventArgs args)
        {
            var rootFrame = await InitializeFrameAsync(args);

            string tileId = AppManifestHelper.GetApplicationId();

            if (rootFrame != null && (rootFrame.Content == null || (args != null && args.TileId != tileId)))
            {
                OnLaunchApplication(args);
            }

            Window.Current.Activate();
        }

        protected override void OnWindowCreated(WindowCreatedEventArgs args)
        {
            if (AppManifestHelper.IsSearchDeclared())
            {
                SearchPane.GetForCurrentView().QuerySubmitted += OnQuerySubmitted;
            }
        }

        protected async override void OnSearchActivated(SearchActivatedEventArgs args)
        {
            var rootFrame = await InitializeFrameAsync(args);

            if (rootFrame != null)
            {
                var searchQueryArguments = new SearchQueryArguments(args);
                OnSearchApplication(searchQueryArguments);
            }

            Window.Current.Activate();
        }

        private async Task<Frame> InitializeFrameAsync(IActivatedEventArgs args)
        {
            var rootFrame = Window.Current.Content as Frame;
            if (rootFrame == null)
            {
                rootFrame = new Frame();
                var frameFacade = new FrameFacadeAdapter(rootFrame);

                SessionStateService = new SessionStateService();
                
                VisualStateAwarePage.GetSessionStateForFrame =
                    frame => SessionStateService.GetSessionStateForFrame(frameFacade);

                SessionStateService.RegisterFrame(frameFacade, "AppFrame");

                NavigationService = CreateNavigationService(frameFacade, SessionStateService);

                SettingsPane.GetForCurrentView().CommandsRequested += OnCommandsRequested;

                ViewModelLocator.SetDefaultViewModelFactory(Resolve);

                OnRegisterKnownTypesForSerialization();
                if (args.PreviousExecutionState == ApplicationExecutionState.Terminated)
                {
                    await SessionStateService.RestoreSessionStateAsync();
                }

                OnInitialize(args);
                if (args.PreviousExecutionState == ApplicationExecutionState.Terminated)
                {
                    try
                    {
                        SessionStateService.RestoreFrameState();
                        NavigationService.RestoreSavedNavigation();
                    }
                    catch (SessionStateServiceException)
                    {
                    }
                }

                Window.Current.Content = rootFrame;
            }

            return rootFrame;
        }

        private INavigationService CreateNavigationService(IFrameFacade rootFrame, ISessionStateService sessionStateService)
        {
            var navigationService = new FrameNavigationService(rootFrame, GetPageType, sessionStateService);
            return navigationService;
        }

        private async void OnSuspending(object sender, SuspendingEventArgs e)
        {
            IsSuspending = true;
            try
            {
                var deferral = e.SuspendingOperation.GetDeferral();

                NavigationService.Suspending();

                await SessionStateService.SaveAsync();

                deferral.Complete();
            }
            finally
            {
                IsSuspending = false;
            }
        }

        private void OnQuerySubmitted(SearchPane sender, SearchPaneQuerySubmittedEventArgs args)
        {
            var searchQueryArguments = new SearchQueryArguments(args);
            OnSearchApplication(searchQueryArguments);
        }

        private void OnCommandsRequested(SettingsPane sender, SettingsPaneCommandsRequestedEventArgs args)
        {
            if (args == null || args.Request == null || args.Request.ApplicationCommands == null)
            {
                return;
            }

            var applicationCommands = args.Request.ApplicationCommands;
            var settingsCommands = GetSettingsCommands();

            foreach (var settingsCommand in settingsCommands)
            {
                applicationCommands.Add(settingsCommand);
            }
        }
    }
}
