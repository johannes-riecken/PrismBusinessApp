

using Microsoft.Practices.Prism.PubSubEvents;
using System;
using System.Globalization;
using Microsoft.Practices.Prism.StoreApps;
using Microsoft.Practices.Prism.StoreApps.Interfaces;

namespace EventAggregatorQuickstart
{

    public class Bootstrapper
    {
        private IEventAggregator _eventAggregator;

        public void Bootstrap(INavigationService navService)
        {
            _eventAggregator  = new EventAggregator();
            ViewModelLocator.Register(typeof(MainPage).ToString(), () => new MainPageViewModel(_eventAggregator));
        }

        public INavigationService CreateNavigationService(IFrameFacade rootFrame, ISessionStateService sessionStateService)
        {
            Func<string, Type> navigationResolver = (string pageToken) =>
            {
                var viewNamespace = "EventAggregatorQuickstart";

                var viewFullName = string.Format(CultureInfo.InvariantCulture, "{0}.{1}Page", viewNamespace, pageToken);
                var viewType = Type.GetType(viewFullName);

                return viewType;
            };

            var navigationService = new FrameNavigationService(rootFrame, navigationResolver, sessionStateService);
            return navigationService;
        }
    }
}
