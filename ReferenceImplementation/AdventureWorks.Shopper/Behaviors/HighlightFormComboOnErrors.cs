

using System.Collections.ObjectModel;
using System.Linq;
using Windows.UI.Xaml;

namespace AdventureWorks.Shopper.Behaviors
{

    public static class HighlightFormComboOnErrors
    {
        public static DependencyProperty PropertyErrorsProperty =
            DependencyProperty.RegisterAttached("PropertyErrors", typeof(ReadOnlyCollection<string>), typeof(HighlightFormComboOnErrors), new PropertyMetadata(null, OnPropertyErrorsChanged));

        public static ReadOnlyCollection<string> GetPropertyErrors(DependencyObject sender)
        {
            if (sender == null)
            {
                return null;
            }

            return (ReadOnlyCollection<string>)sender.GetValue(PropertyErrorsProperty);
        }

        public static void SetPropertyErrors(DependencyObject sender, ReadOnlyCollection<string> value)
        {
            if (sender == null)
            {
                return;
            }

            sender.SetValue(PropertyErrorsProperty, value);
        }

        private static void OnPropertyErrorsChanged(DependencyObject d, DependencyPropertyChangedEventArgs args)
        {
            if (args == null || args.NewValue == null)
            {
                return;
            }

            var control = (FrameworkElement)d;
            var propertyErrors = (ReadOnlyCollection<string>)args.NewValue;

            Style style = (propertyErrors.Any()) ? (Style)Application.Current.Resources["HighlightComboBoxStyle"] : null;
            control.Style = style;
        }
    }
}
