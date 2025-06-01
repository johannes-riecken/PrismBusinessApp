

using System;

namespace Microsoft.Practices.Prism.StoreApps
{
    public class MvvmNavigatedEventArgs : EventArgs
    {
        public string NavigationMode { get; set; }

        public object Parameter { get; set; }
    }
}