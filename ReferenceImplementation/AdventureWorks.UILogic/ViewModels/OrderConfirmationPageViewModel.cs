

using System.Globalization;
using Microsoft.Practices.Prism.StoreApps;
using Microsoft.Practices.Prism.StoreApps.Interfaces;
using System;

namespace AdventureWorks.UILogic.ViewModels
{
    public class OrderConfirmationPageViewModel : ViewModel
    {
        private readonly IResourceLoader _resourceLoader;

        public OrderConfirmationPageViewModel(INavigationService navigationService, IResourceLoader resourceLoader)
        {
            _resourceLoader = resourceLoader;
        }

        public string OrderConfirmationContent { get; set; }

        public override void OnNavigatedTo(object navigationParameter, Windows.UI.Xaml.Navigation.NavigationMode navigationMode, System.Collections.Generic.Dictionary<string, object> viewModelState)
        {
            OrderConfirmationContent = string.Format(CultureInfo.InvariantCulture, _resourceLoader.GetString("OrderConfirmationContent"), navigationParameter);
        }
    }
}