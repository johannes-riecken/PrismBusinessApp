

using System;
using System.Globalization;
using AdventureWorks.UILogic.Models;
using AdventureWorks.UILogic.Repositories;
using AdventureWorks.UILogic.Services;
using Microsoft.Practices.Prism.PubSubEvents;
using System.IO;
using Microsoft.Practices.Prism.StoreApps;
using Microsoft.Practices.Prism.StoreApps.Interfaces;

namespace AdventureWorks.UILogic.ViewModels
{
    public class ShoppingCartTabUserControlViewModel : BindableBase
    {
        private readonly IShoppingCartRepository _shoppingCartRepository;
        private readonly IAlertMessageService _alertMessageService;
        private readonly IResourceLoader _resourceLoader;
        private readonly IAccountService _accountService;
        private int _itemCount;

        public ShoppingCartTabUserControlViewModel(IShoppingCartRepository shoppingCartRepository, IEventAggregator eventAggregator, IAlertMessageService alertMessageService, IResourceLoader resourceLoader, IAccountService accountService)
        {
            _itemCount = 0; //ItemCount will be set using async method call.

            _shoppingCartRepository = shoppingCartRepository;
            _alertMessageService = alertMessageService;
            _resourceLoader = resourceLoader;
            _accountService = accountService;

            if (eventAggregator != null)
            {
                eventAggregator.GetEvent<ShoppingCartUpdatedEvent>().Subscribe(UpdateItemCountAsync);
                eventAggregator.GetEvent<ShoppingCartItemUpdatedEvent>().Subscribe(UpdateItemCountAsync);
            }

            UpdateItemCountAsync(null);
        }

        private async void UpdateItemCountAsync(object notUsed)
        {
            ShoppingCart shoppingCart = null;
            string errorMessage = string.Empty;

            try
            {
                await _accountService.VerifyUserAuthenticationAsync();
                shoppingCart = await _shoppingCartRepository.GetShoppingCartAsync();
            }
            catch(FileNotFoundException){}
            catch(UnauthorizedAccessException){}
            catch(Exception ex)
            {
                errorMessage = string.Format(CultureInfo.CurrentCulture, _resourceLoader.GetString("GeneralServiceErrorMessage"), Environment.NewLine, ex.Message);
            }

            if (!string.IsNullOrWhiteSpace(errorMessage))
            {
                await _alertMessageService.ShowAsync(errorMessage, _resourceLoader.GetString("ErrorServiceUnreachable"));
            }

            if (shoppingCart == null)
            {
                ItemCount = 0;
                return;
            }

            var itemCount = 0;
            if (shoppingCart.ShoppingCartItems != null)
            {
                foreach (var shoppingCartItem in shoppingCart.ShoppingCartItems)
                {
                    itemCount += shoppingCartItem.Quantity;
                }
            }
            ItemCount = itemCount;
        }

        public int ItemCount
        {
            get { return _itemCount; }
            private set { SetProperty(ref _itemCount, value); }
        }
    }
}
