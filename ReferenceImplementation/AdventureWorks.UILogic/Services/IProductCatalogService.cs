

using System.Collections.ObjectModel;
using System.Threading.Tasks;
using AdventureWorks.UILogic.Models;

namespace AdventureWorks.UILogic.Services
{
    public interface IProductCatalogService
    {
        Task<ReadOnlyCollection<Category>> GetCategoriesAsync(int parentId, int maxAmountOfProducts);

        Task<SearchResult> GetFilteredProductsAsync(string productsQueryString);

        Task<ReadOnlyCollection<Product>> GetProductsAsync(int categoryId);

        Task<Category> GetCategoryAsync(int categoryId);

        Task<Product> GetProductAsync(string productNumber);
    }
}

