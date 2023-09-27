

using Microsoft.Practices.Prism.PubSubEvents;
using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.Practices.Prism.StoreApps;

namespace EventAggregatorQuickstart
{

    public class PublisherViewModel : ViewModel
    {
        IEventAggregator _eventAggregator;
        ShoppingCart _cart = new ShoppingCart();

        public PublisherViewModel(IEventAggregator eventAggregator)
        {
            _eventAggregator = eventAggregator;
            AddItemToCartUIThreadCommand = new DelegateCommand(PublishOnUIThread);
            AddItemToCartBackgroundThreadCommand = new DelegateCommand(PublishOnBackgroundThread);
            Debug.WriteLine(String.Format("UI thread is: {0}", Environment.CurrentManagedThreadId));
        }

        public DelegateCommand AddItemToCartUIThreadCommand { get; private set; }
        public DelegateCommand AddItemToCartBackgroundThreadCommand { get; private set; }

        private void PublishOnUIThread()
        {
            AddItemToCart();
            _eventAggregator.GetEvent<ShoppingCartChangedEvent>().Publish(_cart);
        }

        private void PublishOnBackgroundThread()
        {
            AddItemToCart();
            Task.Factory.StartNew(() => 
                {
                    _eventAggregator.GetEvent<ShoppingCartChangedEvent>().Publish(_cart);
                    Debug.WriteLine(String.Format("Publishing from thread: {0}", Environment.CurrentManagedThreadId));
                });
        }

        private void AddItemToCart()
        {
            var item = new ShoppingCartItem("Widget", 19.99m);
            _cart.AddItem(item);
        }
    }
}
