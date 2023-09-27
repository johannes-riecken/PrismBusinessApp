

using System;
using System.Globalization;
using Windows.UI.Core;
using Windows.UI.Popups;

namespace EventAggregatorQuickstart
{
    public class BackgroundSubscriber
    {
        CoreDispatcher _dispatcher;
        public BackgroundSubscriber(CoreDispatcher dispatcher)
        {
            _dispatcher = dispatcher;
        }

        public void HandleShoppingCartChanged(ShoppingCart cart)
        {
            var threadId = Environment.CurrentManagedThreadId;
            var count = cart.Count;

            var dialogAction = _dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                {
                    MessageDialog dialog = new MessageDialog(string.Format(CultureInfo.InvariantCulture, 
                        "Shopping cart updated to {0} item(s) in background subscriber on thread {1}", count, threadId));
                    var showAsync = dialog.ShowAsync();
                });
        }
    }
}
