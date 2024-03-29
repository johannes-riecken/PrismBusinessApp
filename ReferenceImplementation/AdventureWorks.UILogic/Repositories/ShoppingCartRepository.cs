

using AdventureWorks.UILogic.Models;
using AdventureWorks.UILogic.Services;
using Microsoft.Practices.Prism.PubSubEvents;
using System;
using System.Threading.Tasks;
using Microsoft.Practices.Prism.StoreApps.Interfaces;

namespace AdventureWorks.UILogic.Repositories
{
    public class ShoppingCartRepository : IShoppingCartRepository
    {
        public const string ShoppingCartIdKey = "ShoppingCartId";
        private readonly IShoppingCartService _shoppingCartService;
        private readonly IAccountService _accountService;
        private readonly IEventAggregator _eventAggregator;
        private readonly ISessionStateService _sessionStateService;

        private string _shoppingCartId;
        private ShoppingCart _cachedShoppingCart = null;
        
        public ShoppingCartRepository(IShoppingCartService shoppingCartService, IAccountService accountService, IEventAggregator eventAggregator, ISessionStateService sessionStateService)
        {
            _shoppingCartService = shoppingCartService;
            _accountService = accountService;
            _eventAggregator = eventAggregator;
            _sessionStateService = sessionStateService;
            
            if (accountService != null)
            {
                _accountService.UserChanged += _accountService_UserChanged;
            }

            if (_sessionStateService != null && _sessionStateService.SessionState.ContainsKey(ShoppingCartIdKey))
            {
                _shoppingCartId = _sessionStateService.SessionState[ShoppingCartIdKey].ToString();
            }
            else
            {
                _shoppingCartId = Guid.NewGuid().ToString();
                _sessionStateService.SessionState[ShoppingCartIdKey] = _shoppingCartId;
            }
        }

        public async Task ClearCartAsync()
        {
            await _shoppingCartService.DeleteShoppingCartAsync(_shoppingCartId);
            _cachedShoppingCart = null;
            RaiseShoppingCartUpdated();
        }

        async void _accountService_UserChanged(object sender, UserChangedEventArgs e)
        {
            var shoppingCartMerged = false;
            _cachedShoppingCart = null;
            if (e.NewUserInfo != null)
            {
                if (e.OldUserInfo == null)
                {
                    shoppingCartMerged = await _shoppingCartService.MergeShoppingCartsAsync(_shoppingCartId, e.NewUserInfo.UserName);
                }
                _shoppingCartId = e.NewUserInfo.UserName;
            }
            else
            {
                _shoppingCartId = Guid.NewGuid().ToString();
            }

            _sessionStateService.SessionState[ShoppingCartIdKey] = _shoppingCartId;
            RaiseShoppingCartUpdated();
            
            if (shoppingCartMerged)
            {
            }
        }

        public async Task<ShoppingCart> GetShoppingCartAsync()
        {
            if (_cachedShoppingCart != null) return _cachedShoppingCart;

            _cachedShoppingCart = await _shoppingCartService.GetShoppingCartAsync(_shoppingCartId);

            return _cachedShoppingCart;
        }

        public async Task AddProductToShoppingCartAsync(string productId)
        {
            _cachedShoppingCart = null;
            await _shoppingCartService.AddProductToShoppingCartAsync(_shoppingCartId, productId);
            RaiseShoppingCartItemUpdated();
        }

        public async Task RemoveProductFromShoppingCartAsync(string productId)
        {
            _cachedShoppingCart = null;
            await _shoppingCartService.RemoveProductFromShoppingCartAsync(_shoppingCartId, productId);
            RaiseShoppingCartItemUpdated();
        }

        public async Task RemoveShoppingCartItemAsync(string itemId)
        {
            _cachedShoppingCart = null;
            await _shoppingCartService.RemoveShoppingCartItemAsync(_shoppingCartId, itemId);
            RaiseShoppingCartItemUpdated();
        }

        private void RaiseShoppingCartUpdated()
        {
            _eventAggregator.GetEvent<ShoppingCartUpdatedEvent>().Publish(null);
        }

        private void RaiseShoppingCartItemUpdated()
        {
            _eventAggregator.GetEvent<ShoppingCartItemUpdatedEvent>().Publish(null);
        }
    }
}
