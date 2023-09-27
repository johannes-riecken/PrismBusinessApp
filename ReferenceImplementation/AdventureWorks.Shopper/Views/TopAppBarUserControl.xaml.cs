

using System.Collections.Generic;
using Microsoft.Practices.Prism.StoreApps;
using Windows.UI.Core;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;


namespace AdventureWorks.Shopper.Views
{
    public sealed partial class TopAppBarUserControl : UserControl
    {
        private List<Control> _visualStateAwareControls;

        public TopAppBarUserControl()
        {
            this.InitializeComponent();
            if (Windows.ApplicationModel.DesignMode.DesignModeEnabled) return;

            this.Loaded += this.StartLayoutUpdates;

            this.Unloaded += this.StopLayoutUpdates;
        }

        public void StartLayoutUpdates(object sender, RoutedEventArgs eventArgs)
        {
            var control = sender as Control;
            if (control == null) return;
            if (this._visualStateAwareControls == null)
            {
                Window.Current.SizeChanged += this.WindowSizeChanged;
                this._visualStateAwareControls = new List<Control>();

                _visualStateAwareControls.Add(HomeAppBarButton);
                _visualStateAwareControls.Add(ShoppingCartAppBarButton);
            }
            this._visualStateAwareControls.Add(control);

            foreach (var layoutAwareControl in this._visualStateAwareControls)
            {
                VisualStateManager.GoToState(layoutAwareControl, DetermineVisualState(), false);
            }
        }

        private void WindowSizeChanged(object sender, WindowSizeChangedEventArgs e)
        {
            this.InvalidateVisualState();
        }

        public void StopLayoutUpdates(object sender, RoutedEventArgs eventArgs)
        {
            this._visualStateAwareControls.Clear();
            this._visualStateAwareControls = null;
            Window.Current.SizeChanged -= this.WindowSizeChanged;
        }

        private static string DetermineVisualState()
        {
            return "Landscape";
        }

        public void InvalidateVisualState()
        {
            if (this._visualStateAwareControls != null)
            {
                string visualState = DetermineVisualState();
                foreach (var layoutAwareControl in this._visualStateAwareControls)
                {
                    VisualStateManager.GoToState(layoutAwareControl, visualState, false);
                }
            }
        }
    }
}
