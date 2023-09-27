

using Microsoft.Practices.Prism.PubSubEvents;
using System;
using Microsoft.Practices.Prism.StoreApps;
using Windows.UI.Xaml;

namespace EventAggregatorQuickstart.ViewModels
{

    public class SubscriberViewModel : ViewModel
    {
        private bool _showWarning;
        private int _itemsInCart = 0;
        private BackgroundSubscriber _subscriber;
        private IEventAggregator _eventAggregator;

        public SubscriberViewModel(IEventAggregator eventAggregator)
        {
            _eventAggregator = eventAggregator;
            AddBackgroundSubscriberCommand = new DelegateCommand(AddBackgroundSubscriber);
            GCBackgroundSubscriberCommand = new DelegateCommand(GCBackgroundSubscriber);

            _eventAggregator.GetEvent<ShoppingCartChangedEvent>().Subscribe(HandleShoppingCartUpdate, ThreadOption.UIThread);
            _eventAggregator.GetEvent<ShoppingCartChangedEvent>().Subscribe(HandleShoppingCartUpdateFiltered, ThreadOption.UIThread, false, IsCartCountPossiblyTooHigh);
        }

        public DelegateCommand AddBackgroundSubscriberCommand { get; private set; }
        public DelegateCommand GCBackgroundSubscriberCommand { get; private set; }

        public bool ShowWarning
        {
            get { return _showWarning; }
            set { SetProperty(ref _showWarning, value); }
        }

        public int ItemsInCart
        {
            get { return _itemsInCart; }
            set { SetProperty(ref _itemsInCart, value); }
        }

        private void HandleShoppingCartUpdate(ShoppingCart cart)
        {
            ItemsInCart = cart.Count;
        }

        private void HandleShoppingCartUpdateFiltered(ShoppingCart cart)
        {
            ShowWarning = true;
        }

        private void AddBackgroundSubscriber()
        {
            if (_subscriber != null) return;

            _subscriber = new BackgroundSubscriber(Window.Current.Dispatcher);
            _eventAggregator.GetEvent<ShoppingCartChangedEvent>().Subscribe(_subscriber.HandleShoppingCartChanged);
        }

        private void GCBackgroundSubscriber()
        {
            _subscriber = null;
            GC.Collect();
        }

        private bool IsCartCountPossiblyTooHigh(ShoppingCart shoppingCart)
        {
            return (shoppingCart.Count > 10);
        }
    }
}
