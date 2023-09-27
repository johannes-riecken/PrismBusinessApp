

using System.Collections.ObjectModel;

namespace AdventureWorks.UILogic.Models
{
    public class SearchResult
    {
        public SearchResult(int totalCount, ReadOnlyCollection<Product> products)
        {
            TotalCount = totalCount;
            Products = products;
        }
        public int TotalCount { get; private set; }
        public ReadOnlyCollection<Product> Products { get; private set; }
    }
}
