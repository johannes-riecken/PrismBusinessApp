

using AdventureWorks.WebServices.Models;
using AdventureWorks.WebServices.Repositories;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Http;

namespace AdventureWorks.WebServices.Controllers
{
    public class ProductController : ApiController
    {
        private const int MaxSearchResults = 1000;

        private IProductRepository _productRepository;

        public ProductController()
            : this(new ProductRepository())
        { }

        public ProductController(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public IEnumerable<Product> GetProducts()
        {
            return _productRepository.GetProducts();
        }

        public Product GetProduct(string id)
        {
            var item = _productRepository.GetProduct(id);

            if (item == null)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }

            return item;
        }

        public SearchResult GetSearchResults(string queryString)
        {
            var fullsearchResult = _productRepository.GetProducts().Where(p => p.Title.ToUpperInvariant().Contains(queryString.ToUpperInvariant()));

            var searchResult = new SearchResult
                                   {
                                       TotalCount = fullsearchResult.Count(),
                                       Products = fullsearchResult.Take(MaxSearchResults)
                                   };

            return searchResult;

        }

        public IEnumerable<Product> GetProducts(int categoryId)
        {
            if (categoryId == 0)
            {
                return _productRepository.GetTodaysDealsProducts();
            }

            return _productRepository.GetProductsForCategory(categoryId);
        }
    }
}
