

using EventAggregatorQuickstart.ViewModels;
using Microsoft.Practices.Prism.PubSubEvents;
using System;
using Microsoft.Practices.Prism.StoreApps;

namespace EventAggregatorQuickstart
{

    public class MainPageViewModel : ViewModel
    {
        public MainPageViewModel(IEventAggregator eventAggregator)
        {
            SubscriberViewModel = new SubscriberViewModel(eventAggregator);
            PublisherViewModel = new PublisherViewModel(eventAggregator);
        }

        public SubscriberViewModel SubscriberViewModel { get; set; }
        public PublisherViewModel PublisherViewModel { get; set; }

        public string ThreadMessage { get { return string.Format("UI Thread ID: {0}", Environment.CurrentManagedThreadId); } }
    }
}
