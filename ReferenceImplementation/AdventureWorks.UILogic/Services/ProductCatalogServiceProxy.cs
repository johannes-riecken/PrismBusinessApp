

using System.Collections.ObjectModel;
using System.Globalization;
using System.Threading.Tasks;
using AdventureWorks.UILogic.Models;
using Microsoft.Practices.Prism.StoreApps;
using Newtonsoft.Json;
using Windows.Web.Http;
using System;

namespace AdventureWorks.UILogic.Services
{
    public class ProductCatalogServiceProxy : IProductCatalogService
    {
        private string _productsBaseUrl = string.Format(CultureInfo.InvariantCulture, "{0}/api/Product/", Constants.ServerAddress);
        private string _categoriesBaseUrl = string.Format(CultureInfo.InvariantCulture, "{0}/api/Category/", Constants.ServerAddress);

        public async Task<ReadOnlyCollection<Category>> GetCategoriesAsync(int parentId, int maxAmountOfProducts)
        {
            using (var httpClient = new HttpClient())
            {
                var response = await httpClient.GetAsync(new Uri(string.Format("{0}?parentId={1}&maxAmountOfProducts={2}", _categoriesBaseUrl, parentId, maxAmountOfProducts)));
                response.EnsureSuccessStatusCode();
                var responseContent = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<ReadOnlyCollection<Category>>(responseContent);

                return result;
            }
        }

        public async Task<SearchResult> GetFilteredProductsAsync(string productsQueryString)
        {
            using (var httpClient = new HttpClient())
            {
                var response = await httpClient.GetAsync(new Uri(string.Format("{0}?queryString={1}", _productsBaseUrl, productsQueryString)));
                response.EnsureSuccessStatusCode();
                var responseContent = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<SearchResult>(responseContent);

                return result;
            }
        }

        public async Task<ReadOnlyCollection<Product>> GetProductsAsync(int categoryId)
        {
            using (var httpClient = new HttpClient())
            {
                var response =
                    await httpClient.GetAsync(new Uri(string.Format("{0}?categoryId={1}", _productsBaseUrl, categoryId)));
                response.EnsureSuccessStatusCode();
                var responseContent = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<ReadOnlyCollection<Product>>(responseContent);

                return result;
            }
        }

        public async Task<Category> GetCategoryAsync(int categoryId)
        {
            using (var httpClient = new HttpClient())
            {
                var response = await httpClient.GetAsync(new Uri(_categoriesBaseUrl + categoryId.ToString()));
                response.EnsureSuccessStatusCode();
                var responseContent = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<Category>(responseContent);

                return result;
            }
        }

        public async Task<Product> GetProductAsync(string productNumber)
        {
            using (var httpClient = new HttpClient())
            {
                var response = await httpClient.GetAsync(new Uri(_productsBaseUrl + productNumber));
                response.EnsureSuccessStatusCode();
                var responseContent = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<Product>(responseContent);

                return result;
            }
        }
    }
}
