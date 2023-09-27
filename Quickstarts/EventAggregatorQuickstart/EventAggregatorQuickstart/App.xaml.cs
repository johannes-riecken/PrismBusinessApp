

using System;
using Microsoft.Practices.Prism.StoreApps;
using Microsoft.Practices.Prism.StoreApps.Interfaces;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;



namespace EventAggregatorQuickstart
{
    sealed partial class App : Application
    {
        Bootstrapper _bootstrapper = new Bootstrapper();
        INavigationService _navigationService;
        public App()
        {
            this.InitializeComponent();
            this.Suspending += OnSuspending;
        }

        protected override void OnLaunched(LaunchActivatedEventArgs args)
        {
            Frame rootFrame = Window.Current.Content as Frame;

            if (rootFrame == null)
            {
                rootFrame = new Frame();
                var frameFacade = new FrameFacadeAdapter(rootFrame);
                _navigationService = _bootstrapper.CreateNavigationService(frameFacade, new SessionStateService());
                _bootstrapper.Bootstrap(_navigationService);

                if (args.PreviousExecutionState == ApplicationExecutionState.Terminated)
                {
                    _navigationService.RestoreSavedNavigation();
                }

                Window.Current.Content = rootFrame;
            }

            if (rootFrame.Content == null)
            {
                if (!_navigationService.Navigate("Main", args.Arguments))
                {
                    throw new Exception("Failed to create initial page");
                }
            }
            Window.Current.Activate();
        }

        private void OnSuspending(object sender, SuspendingEventArgs e)
        {
            var deferral = e.SuspendingOperation.GetDeferral();
            _navigationService.Suspending();

            deferral.Complete();
        }
    }
}
