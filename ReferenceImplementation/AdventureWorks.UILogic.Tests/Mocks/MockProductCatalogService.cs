

using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using AdventureWorks.UILogic.Models;
using AdventureWorks.UILogic.Services;

namespace AdventureWorks.UILogic.Tests.Mocks
{
    public class MockProductCatalogService : IProductCatalogService
    {
        public Func<int, int, Task<ReadOnlyCollection<Category>>> GetSubcategoriesAsyncDelegate { get; set; }
        public Func<string, Task<SearchResult>> GetFilteredProductsAsyncDelegate { get; set; }
        public Func<int, Task<ReadOnlyCollection<Product>>> GetProductsAsyncDelegate { get; set; }
        public Func<int, Task<Category>> GetCategoryAsyncDelegate { get; set; }
        public Func<string, Task<Product>> GetProductAsyncDelegate { get; set; }

        public Task<ReadOnlyCollection<Category>> GetCategoriesAsync(int parentId, int maxAmountOfProducts)
        {
            return this.GetSubcategoriesAsyncDelegate(parentId, maxAmountOfProducts);
        }

        public Task<SearchResult> GetFilteredProductsAsync(string productsQueryString)
        {
            return this.GetFilteredProductsAsyncDelegate(productsQueryString);
        }

        public Task<ReadOnlyCollection<Product>> GetProductsAsync(int categoryId)
        {
            return this.GetProductsAsyncDelegate(categoryId);
        }

        public Task<Category> GetCategoryAsync(int categoryId)
        {
            return this.GetCategoryAsyncDelegate(categoryId);
        }

        public Task<Product> GetProductAsync(string productNumber)
        {
            return this.GetProductAsyncDelegate(productNumber);
        }
    }
}