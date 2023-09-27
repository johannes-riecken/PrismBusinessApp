

using System.Collections.ObjectModel;
using System.Threading.Tasks;
using AdventureWorks.UILogic.Models;

namespace AdventureWorks.UILogic.Repositories
{
    public interface IProductCatalogRepository
    {
        Task<ReadOnlyCollection<Category>> GetRootCategoriesAsync(int maxAmountOfProducts);

        Task<ReadOnlyCollection<Category>> GetSubcategoriesAsync(int parentId, int maxAmountOfProducts);

        Task<SearchResult> GetFilteredProductsAsync(string productsQueryString);

        Task<ReadOnlyCollection<Product>> GetProductsAsync(int categoryId);

        Task<Category> GetCategoryAsync(int categoryId);

        Task<Product> GetProductAsync(string productNumber);

        string GetCategoryName(int parentId);
    }
}