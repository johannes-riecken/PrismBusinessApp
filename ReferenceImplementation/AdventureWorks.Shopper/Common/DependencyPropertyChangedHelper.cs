

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace AdventureWorks.Shopper.Common
{
    public class DependencyPropertyChangedHelper : DependencyObject
    {
        public DependencyPropertyChangedHelper(DependencyObject source, string propertyPath)
        {
            Binding binding = new Binding
            {
                Source = source,
                Path = new PropertyPath(propertyPath)
            };
            BindingOperations.SetBinding(this, HelperProperty, binding);
        }

        public static DependencyProperty HelperProperty =
            DependencyProperty.Register("Helper", typeof(object), typeof(DependencyPropertyChangedHelper), new PropertyMetadata(null, OnPropertyChanged));

        public object Helper
        {
            get { return (object)GetValue(HelperProperty); }
            set { SetValue(HelperProperty, value); }
        }

        private static void OnPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var helper = (DependencyPropertyChangedHelper)d;
            helper.PropertyChanged(d, e);
        }

        public event EventHandler<DependencyPropertyChangedEventArgs> PropertyChanged = delegate { };
    }
}
