

using System.Web.Http;
using AdventureWorks.WebServices.Models;
using AdventureWorks.WebServices.Repositories;
using System;
using System.Linq;
using System.Net;
using System.Net.Http;

namespace AdventureWorks.WebServices.Controllers
{
    public class ShoppingCartController : ApiController
    {
        private IShoppingCartRepository _shoppingCartRepository;
        private IProductRepository _productRepository;
        private static object _lock = new object();

        public ShoppingCartController()
            : this(new ShoppingCartRepository(), new ProductRepository())
        { }

        public ShoppingCartController(IShoppingCartRepository shoppingCartRepository, IProductRepository productRepository)
        {
            _shoppingCartRepository = shoppingCartRepository;
            _productRepository = productRepository;
        }

        public ShoppingCart Get(string id)
        {
            return _shoppingCartRepository.GetById(id);
        }

        [HttpPost]
        public void AddProductToShoppingCart(string id, string productIdToIncrement)
        {
            lock (_lock)
            {
                var product = _productRepository.GetProducts().FirstOrDefault(c => c.ProductNumber == productIdToIncrement);
                if (product == null)
                {
                    throw new HttpResponseException(HttpStatusCode.NotFound);
                }

                _shoppingCartRepository.AddProductToCart(id, product);
            }
        }

        [HttpPost]
        public void RemoveProductFromShoppingCart(string id, string productIdToDecrement)
        {
            lock (_lock)
            {
                var product = _productRepository.GetProducts().FirstOrDefault(c => c.ProductNumber == productIdToDecrement);
                if (product == null)
                {
                    throw new HttpResponseException(HttpStatusCode.NotFound);
                }

                if (!_shoppingCartRepository.RemoveProductFromCart(id, productIdToDecrement))
                {
                    throw new HttpResponseException(HttpStatusCode.NotFound);
                }
            }
        }

        public void DeleteShoppingCart(string id)
        {
            if (!_shoppingCartRepository.Delete(id))
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }
        }

        [HttpPut]
        public void RemoveShoppingCartItem(string id, string itemIdToRemove)
        {
            lock (_lock)
            {
                ShoppingCart shoppingCart = _shoppingCartRepository.GetById(id);

                if (shoppingCart == null)
                {
                    throw new HttpResponseException(HttpStatusCode.NotFound);
                }

                if (!_shoppingCartRepository.RemoveItemFromCart(shoppingCart, itemIdToRemove))
                {
                    throw new HttpResponseException(HttpStatusCode.NotFound);
                }
            }
        }

        [HttpPost]
        public bool MergeShoppingCarts(string id, string anonymousShoppingCartId)
        {
            var shoppingCartMerged = false;
            lock (_lock)
            {
                if (id == anonymousShoppingCartId) return false;
                var anonymousShoppingCart = _shoppingCartRepository.GetById(anonymousShoppingCartId);
                var authenticatedShoppingCart = _shoppingCartRepository.GetById(id);
                if ((authenticatedShoppingCart != null && authenticatedShoppingCart.ShoppingCartItems.Count > 0) 
                    && (anonymousShoppingCart != null && anonymousShoppingCart.ShoppingCartItems.Count > 0))
                {
                    shoppingCartMerged = true;
                }

                if (anonymousShoppingCart != null)
                {
                    foreach (var shoppingCartItem in anonymousShoppingCart.ShoppingCartItems)
                    {
                        for (int i = 0; i < shoppingCartItem.Quantity; i++)
                        {
                            _shoppingCartRepository.AddProductToCart(id, shoppingCartItem.Product);
                        }
                    }

                    _shoppingCartRepository.Delete(anonymousShoppingCartId);
                }
                return shoppingCartMerged;
            }
        }

        [HttpPost]
        public void Reset(bool resetData)
        {
            if (resetData)
                ShoppingCartRepository.Reset();
        }
    }
}
