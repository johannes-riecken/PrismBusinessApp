

using System;
using System.Collections.Generic;
using Microsoft.Practices.Prism.StoreApps.Interfaces;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Navigation;

namespace Microsoft.Practices.Prism.StoreApps
{
    public class FrameFacadeAdapter : IFrameFacade
    {
        private readonly Windows.UI.Xaml.Controls.Frame _frame;
        private readonly List<EventHandler<MvvmNavigatedEventArgs>> _navigatedEventHandlers = new List<EventHandler<MvvmNavigatedEventArgs>>();
        private readonly List<EventHandler> _navigatingEventHandlers = new List<EventHandler>();

        public FrameFacadeAdapter(Windows.UI.Xaml.Controls.Frame frame)
        {
            _frame = frame;
        }

        public object Content
        {
            get { return _frame.Content; }
            set { _frame.Content = value; }
        }

        public void GoBack()
        {
            _frame.GoBack();
        }

        public string GetNavigationState()
        {
            var navigationState = _frame.GetNavigationState();
            return navigationState;
        }

        public void SetNavigationState(string navigationState)
        {
            _frame.SetNavigationState(navigationState);
        }

        public bool Navigate(Type sourcePageType, object parameter)
        {
            return _frame.Navigate(sourcePageType, parameter);
        }

        public int BackStackDepth
        {
            get { return _frame.BackStackDepth; }
        }

        public bool CanGoBack
        {
            get { return _frame.CanGoBack; }
        }

        public event EventHandler<MvvmNavigatedEventArgs> Navigated
        {
            add
            {
                if (_navigatedEventHandlers.Contains(value)) return;
                _navigatedEventHandlers.Add(value);

                if (_navigatedEventHandlers.Count == 1)
                {
                    _frame.Navigated += FacadeNavigatedEventHandler;
                }
            }

            remove
            {
                if (!_navigatedEventHandlers.Contains(value)) return;
                _navigatedEventHandlers.Remove(value);

                if (_navigatedEventHandlers.Count == 0)
                {
                    _frame.Navigated -= FacadeNavigatedEventHandler;
                }
            }
        }

        public event EventHandler Navigating
        {
            add
            {
                if (_navigatingEventHandlers.Contains(value)) return;
                _navigatingEventHandlers.Add(value);

                if (_navigatingEventHandlers.Count == 1)
                {
                    _frame.Navigating += FacadeNavigatingCancelEventHandler;
                }
            }

            remove
            {
                if (!_navigatingEventHandlers.Contains(value)) return;
                _navigatingEventHandlers.Remove(value);

                if (_navigatingEventHandlers.Count == 0)
                {
                    _frame.Navigating -= FacadeNavigatingCancelEventHandler;
                }
            }
        }

        public object GetValue(DependencyProperty dependencyProperty)
        {
            return _frame.GetValue(dependencyProperty);
        }

        public void SetValue(DependencyProperty dependencyProperty, object value)
        {
            _frame.SetValue(dependencyProperty, value);
        }

        public void ClearValue(DependencyProperty dependencyProperty)
        {
            _frame.ClearValue(dependencyProperty);
        }

        private void FacadeNavigatedEventHandler(object sender, NavigationEventArgs e)
        {
            foreach (var handler in _navigatedEventHandlers)
            {
                var eventArgs = new MvvmNavigatedEventArgs()
                {
                    NavigationMode = e.NavigationMode,
                    Parameter = e.Parameter
                };
                handler(this, eventArgs);
            }
        }

        private void FacadeNavigatingCancelEventHandler(object sender, NavigatingCancelEventArgs e)
        {
            foreach (var handler in _navigatingEventHandlers)
            {
                handler(this, new EventArgs());
            }
        }

    }
}
