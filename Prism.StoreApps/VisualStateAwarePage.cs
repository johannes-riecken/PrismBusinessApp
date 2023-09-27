

using Microsoft.Practices.Prism.StoreApps.Interfaces;
using System;
using System.Collections.Generic;
using Windows.System;
using Windows.UI.Core;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace Microsoft.Practices.Prism.StoreApps
{
    public class VisualStateAwarePage : Page
    {
        public static readonly string DefaultLayoutVisualState = "DefaultLayout";

        public static readonly string PortraitLayoutVisualState = "PortraitLayout";

        public static readonly string MinimalLayoutVisualState = "MinimalLayout";

        public int MinimalLayoutWidth { get; set; }

        public static Func<IFrameFacade, IDictionary<string, object>> GetSessionStateForFrame { get; set; }
 
        private List<Control> _visualStateAwareControls;

        public VisualStateAwarePage()
        {
            MinimalLayoutWidth = 500;

            if (Windows.ApplicationModel.DesignMode.DesignModeEnabled) return;

            this.Loaded += (sender, e) =>
            {
                this.StartLayoutUpdates(sender, e);

                if (this.ActualHeight == Window.Current.Bounds.Height &&
                    this.ActualWidth == Window.Current.Bounds.Width)
                {
                    Window.Current.CoreWindow.Dispatcher.AcceleratorKeyActivated +=
                        CoreDispatcher_AcceleratorKeyActivated;
                    Window.Current.CoreWindow.PointerPressed +=
                        this.CoreWindow_PointerPressed;
                }
            };

            this.Unloaded += (sender, e) =>
            {
                this.StopLayoutUpdates(sender, e);
                Window.Current.CoreWindow.Dispatcher.AcceleratorKeyActivated -=
                    CoreDispatcher_AcceleratorKeyActivated;
                Window.Current.CoreWindow.PointerPressed -=
                    this.CoreWindow_PointerPressed;
            };
        }


        #region Navigation support

        DelegateCommand _goBackCommand;

        public DelegateCommand GoBackCommand
        {
            get
            {
                if (_goBackCommand == null)
                {
                    _goBackCommand = new DelegateCommand(
                        () => this.GoBack(this, null),
                        () => this.CanGoBack());
                }
                return _goBackCommand;
            }
            set
            {
                _goBackCommand = value;
            }
        }
        protected virtual void GoHome(object sender, RoutedEventArgs eventArgs)
        {
            if (this.Frame != null)
            {
                while (this.Frame.CanGoBack) this.Frame.GoBack();
            }
        }

        public virtual bool CanGoBack()
        {
            return this.Frame != null && this.Frame.CanGoBack;
        }

        protected virtual void GoBack(object sender, RoutedEventArgs eventArgs)
        {
            if (this.Frame != null && this.Frame.CanGoBack) this.Frame.GoBack();
        }

        protected virtual void GoForward(object sender, RoutedEventArgs eventArgs)
        {
            if (this.Frame != null && this.Frame.CanGoForward) this.Frame.GoForward();
        }

        private void CoreDispatcher_AcceleratorKeyActivated(CoreDispatcher sender,
            AcceleratorKeyEventArgs args)
        {
            var virtualKey = args.VirtualKey;

            if ((args.EventType == CoreAcceleratorKeyEventType.SystemKeyDown ||
                args.EventType == CoreAcceleratorKeyEventType.KeyDown) &&
                (virtualKey == VirtualKey.Left || virtualKey == VirtualKey.Right ||
                (int)virtualKey == 166 || (int)virtualKey == 167))
            {
                var coreWindow = Window.Current.CoreWindow;
                var downState = CoreVirtualKeyStates.Down;
                bool menuKey = (coreWindow.GetKeyState(VirtualKey.Menu) & downState) == downState;
                bool controlKey = (coreWindow.GetKeyState(VirtualKey.Control) & downState) == downState;
                bool shiftKey = (coreWindow.GetKeyState(VirtualKey.Shift) & downState) == downState;
                bool noModifiers = !menuKey && !controlKey && !shiftKey;
                bool onlyAlt = menuKey && !controlKey && !shiftKey;

                if (((int)virtualKey == 166 && noModifiers) ||
                    (virtualKey == VirtualKey.Left && onlyAlt))
                {
                    args.Handled = true;
                    this.GoBack(this, new RoutedEventArgs());
                }
                else if (((int)virtualKey == 167 && noModifiers) ||
                    (virtualKey == VirtualKey.Right && onlyAlt))
                {
                    args.Handled = true;
                    this.GoForward(this, new RoutedEventArgs());
                }
            }
        }

        private void CoreWindow_PointerPressed(CoreWindow sender,
            PointerEventArgs args)
        {
            var properties = args.CurrentPoint.Properties;

            if (properties.IsLeftButtonPressed || properties.IsRightButtonPressed ||
                properties.IsMiddleButtonPressed) return;

            bool backPressed = properties.IsXButton1Pressed;
            bool forwardPressed = properties.IsXButton2Pressed;
            if (backPressed ^ forwardPressed)
            {
                args.Handled = true;
                if (backPressed) this.GoBack(this, new RoutedEventArgs());
                if (forwardPressed) this.GoForward(this, new RoutedEventArgs());
            }
        }

        #endregion

        #region Visual state switching

        public void StartLayoutUpdates(object sender, RoutedEventArgs eventArgs)
        {
            var control = sender as Control;
            if (control == null) return;
            if (this._visualStateAwareControls == null)
            {
                Window.Current.SizeChanged += this.WindowSizeChanged;
                this._visualStateAwareControls = new List<Control>();
            }
            this._visualStateAwareControls.Add(control);

            VisualStateManager.GoToState(control, DetermineVisualState(this.ActualWidth, this.ActualHeight), false);
        }

        private void WindowSizeChanged(object sender, WindowSizeChangedEventArgs e)
        {
            this.InvalidateVisualState(e.Size.Width, e.Size.Height);
        }

      
        public void StopLayoutUpdates(object sender, RoutedEventArgs eventArgs)
        {
            var control = sender as Control;
            if (control == null || this._visualStateAwareControls == null) return;
            this._visualStateAwareControls.Remove(control);
            if (this._visualStateAwareControls.Count == 0)
            {
                this._visualStateAwareControls = null;
                Window.Current.SizeChanged -= this.WindowSizeChanged;
            }
        }

        protected virtual string DetermineVisualState(double width, double height)
        {
            if (width <= MinimalLayoutWidth) 
            { 
                return MinimalLayoutVisualState; 
            } 
            
            if (width < height) 
            { 
                return PortraitLayoutVisualState; 
            } 
            
            return DefaultLayoutVisualState; 
        }

        public void InvalidateVisualState(double width, double height)
        {
            if (this._visualStateAwareControls != null)
            {
                string visualState = DetermineVisualState(width, height);
                foreach (var layoutAwareControl in this._visualStateAwareControls)
                {
                    VisualStateManager.GoToState(layoutAwareControl, visualState, false);
                }
            }
        }
        #endregion

        #region Process lifetime management

        private String _pageKey;

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (e == null) throw new ArgumentNullException("e");

            if (this._pageKey != null) return;

            var frameFacade = new FrameFacadeAdapter(this.Frame);
            var frameState = GetSessionStateForFrame(frameFacade);
            this._pageKey = "Page-" + frameFacade.BackStackDepth;

            if (e.NavigationMode == NavigationMode.New)
            {
                var nextPageKey = this._pageKey;
                int nextPageIndex = frameFacade.BackStackDepth;
                while (frameState.Remove(nextPageKey))
                {
                    nextPageIndex++;
                    nextPageKey = "Page-" + nextPageIndex;
                }

                this.LoadState(e.Parameter, null);
            }
            else
            {
                this.LoadState(e.Parameter, (Dictionary<String, Object>)frameState[this._pageKey]);
            }
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            var frameFacade = new FrameFacadeAdapter(this.Frame);
            var frameState = GetSessionStateForFrame(frameFacade);
            var pageState = new Dictionary<String, Object>();
            this.SaveState(pageState);
            frameState[_pageKey] = pageState;
        }

        protected virtual void LoadState(Object navigationParameter, Dictionary<String, Object> pageState)
        {
        }

        protected virtual void SaveState(Dictionary<String, Object> pageState)
        {
        }

        #endregion
    }
}
