

using System;
using Windows.UI.Xaml.Navigation;

namespace Microsoft.Practices.Prism.StoreApps
{
    public class MvvmNavigatedEventArgs : EventArgs
    {
        public NavigationMode NavigationMode { get; set; }

        public object Parameter { get; set; }
    }
}