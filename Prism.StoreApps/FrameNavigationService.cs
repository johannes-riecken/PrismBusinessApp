

using System;
using System.Collections.Generic;
using System.Globalization;
using Microsoft.Practices.Prism.StoreApps.Interfaces;
using Windows.ApplicationModel.Resources;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Xaml;

namespace Microsoft.Practices.Prism.StoreApps
{
    public class FrameNavigationService : INavigationService
    {
        private const string LastNavigationParameterKey = "LastNavigationParameter";
        private const string LastNavigationPageKey = "LastNavigationPageKey";
        private readonly IFrameFacade _frame;
        private readonly Func<string, Type> _navigationResolver;
        private readonly ISessionStateService _sessionStateService;

        public FrameNavigationService(IFrameFacade frame, Func<string, Type> navigationResolver, ISessionStateService sessionStateService)
        {
            _frame = frame;
            _navigationResolver = navigationResolver;
            _sessionStateService = sessionStateService;

            if (frame != null)
            {
                _frame.Navigating += frame_Navigating;
                _frame.Navigated += frame_Navigated;
            }
        }

        public bool Navigate(string pageToken, object parameter)
        {
            Type pageType = _navigationResolver(pageToken);

            if (pageType == null)
            {
                var resourceLoader = ResourceLoader.GetForCurrentView(Constants.StoreAppsInfrastructureResourceMapId);
                var error = string.Format(CultureInfo.CurrentCulture, resourceLoader.GetString("FrameNavigationServiceUnableResolveMessage"), pageToken);
                throw new ArgumentException(error, "pageToken");
            }

            var lastNavigationParameter = _sessionStateService.SessionState.ContainsKey(LastNavigationParameterKey) ? _sessionStateService.SessionState[LastNavigationParameterKey] : null;
            var lastPageTypeFullName = _sessionStateService.SessionState.ContainsKey(LastNavigationPageKey) ? _sessionStateService.SessionState[LastNavigationPageKey] as string : string.Empty;

            if (lastPageTypeFullName != pageType.FullName || !AreEquals(lastNavigationParameter, parameter))
            {
                return _frame.Navigate(pageType, parameter);
            }

            return false;
        }


        public void GoBack()
        {
            _frame.GoBack();
        }

        public bool CanGoBack()
        {
            return _frame.CanGoBack;
        }

        public void ClearHistory()
        {
            _frame.SetNavigationState("1,0");
        }

        public void RestoreSavedNavigation()
        {
            var navigationParameter = _sessionStateService.SessionState[LastNavigationParameterKey];
            NavigateToCurrentViewModel(NavigationMode.Refresh, navigationParameter);
        }

        public void Suspending()
        {
            NavigateFromCurrentViewModel(true);
        }

        private void NavigateToCurrentViewModel(NavigationMode navigationMode, object parameter)
        {
            var frameState = _sessionStateService.GetSessionStateForFrame(_frame);
            var viewModelKey = "ViewModel-" + _frame.BackStackDepth;

            if (navigationMode == NavigationMode.New)
            {
                var nextViewModelKey = viewModelKey;
                int nextViewModelIndex = _frame.BackStackDepth;
                while (frameState.Remove(nextViewModelKey))
                {
                    nextViewModelIndex++;
                    nextViewModelKey = "ViewModel-" + nextViewModelIndex;
                }
            }

            var newView = _frame.Content as FrameworkElement;
            if (newView == null) return;
            var newViewModel = newView.DataContext as INavigationAware;
            if (newViewModel != null)
            {
                Dictionary<string, object> viewModelState;
                if (frameState.ContainsKey(viewModelKey))
                {
                    viewModelState = frameState[viewModelKey] as Dictionary<string, object>;
                }
                else
                {
                    viewModelState = new Dictionary<string, object>();
                }
                newViewModel.OnNavigatedTo(parameter, navigationMode, viewModelState);
                frameState[viewModelKey] = viewModelState;
            }
        }

        private void NavigateFromCurrentViewModel(bool suspending)
        {
            var departingView = _frame.Content as FrameworkElement;
            if (departingView == null) return;
            var frameState = _sessionStateService.GetSessionStateForFrame(_frame);
            var departingViewModel = departingView.DataContext as INavigationAware;

            var viewModelKey = "ViewModel-" + _frame.BackStackDepth;
            if (departingViewModel != null)
            {
                var viewModelState = frameState.ContainsKey(viewModelKey)
                                         ? frameState[viewModelKey] as Dictionary<string, object>
                                         : null;

                departingViewModel.OnNavigatedFrom(viewModelState, suspending);
            }
        }

        private void frame_Navigating(object sender, EventArgs e)
        {
            NavigateFromCurrentViewModel(false);
        }

        private void frame_Navigated(object sender, MvvmNavigatedEventArgs e)
        {
            _sessionStateService.SessionState[LastNavigationPageKey] = _frame.Content.GetType().FullName;
            _sessionStateService.SessionState[LastNavigationParameterKey] = e.Parameter;

            NavigateToCurrentViewModel(e.NavigationMode, e.Parameter);
        }

        private static bool AreEquals(object obj1, object obj2)
        {
            if (obj1 != null)
            {
                return obj1.Equals(obj2);
            }
            return obj2 == null;
        }
    }
}
