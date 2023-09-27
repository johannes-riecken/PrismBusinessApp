

using Microsoft.Practices.Prism.StoreApps;
using Microsoft.Practices.Prism.StoreApps.Interfaces;

namespace AdventureWorks.UILogic.ViewModels
{
    public class SearchUserControlViewModel
    {
        private readonly ISearchPaneService _searchPaneService;

        public SearchUserControlViewModel(ISearchPaneService searchPaneService)
        {
            _searchPaneService = searchPaneService;
            SearchCommand = new DelegateCommand(ShowSearchPane);
        }

        public DelegateCommand SearchCommand { get; set; }

        private void ShowSearchPane()
        {
            _searchPaneService.Show();
        }
    }
}
