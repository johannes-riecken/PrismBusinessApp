

using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace AdventureWorks.Shopper.Behaviors
{
    public static class ChangeVisualState
    {
        public static DependencyProperty GoToStateProperty =
  DependencyProperty.RegisterAttached("VisualState", typeof(string), typeof(ChangeVisualState), new PropertyMetadata(null, OnVisualStateChanged));

        public static void SetVisualState(DependencyObject sender, string state)
        {
            if (sender == null)
            {
                return;
            }

            sender.SetValue(GoToStateProperty, state);
        }

        public static string GetVisualState(DependencyObject sender)
        {
            if (sender == null)
            {
                return null;
            }

            return (string)sender.GetValue(GoToStateProperty);
        }

        private static void OnVisualStateChanged(DependencyObject d, DependencyPropertyChangedEventArgs args)
        {
            var userControl = (Control)d;
            var visualState = args.NewValue.ToString();

            if (userControl != null && !string.IsNullOrEmpty(visualState))
            {
                VisualStateManager.GoToState(userControl, visualState, false);
            }
        }
    }
}