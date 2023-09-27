

using System;

namespace Microsoft.Practices.Prism.StoreApps.Interfaces
{
    public interface IFlyoutService
    {
        Func<string, FlyoutView> FlyoutResolver { get; set; }
        
        void ShowFlyout(string flyoutId);

        void ShowFlyout(string flyoutId, object parameter, Action successAction);
    }
}