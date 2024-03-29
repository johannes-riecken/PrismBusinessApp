

using System;
using Microsoft.Practices.Prism.StoreApps;
using Microsoft.Practices.Prism.StoreApps.Interfaces;

namespace AdventureWorks.UILogic.Tests.Mocks
{
    public class MockFlyoutService : IFlyoutService
    {
        public Action<string, object, Action> ShowFlyoutDelegate { get; set; }

        public Func<string, FlyoutView> FlyoutResolver { get; set; }

        public void ShowFlyout(string flyoutId)
        {
            ShowFlyoutDelegate(flyoutId, null, null);
        }

        public void ShowFlyout(string flyoutId, object parameter, Action successAction)
        {
            ShowFlyoutDelegate(flyoutId, parameter, successAction);
        }
    }
}
