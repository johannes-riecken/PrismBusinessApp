

using Windows.ApplicationModel.Activation;
using Windows.ApplicationModel.Search;

namespace Microsoft.Practices.Prism.StoreApps
{
    public class SearchQueryArguments
    {
        public SearchQueryArguments(ISearchActivatedEventArgs args)
        {
            if (args != null)
            {
                Language = args.Language;
                QueryText = args.QueryText;
            }
        }

        public SearchQueryArguments(SearchPaneQuerySubmittedEventArgs args)
        {
            if (args != null)
            {
                Language = args.Language;
                QueryText = args.QueryText;
            }
        }

        public string Language { get; set; }

        public string QueryText { get; set; }
    }
}
