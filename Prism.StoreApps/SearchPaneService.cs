

using Microsoft.Practices.Prism.StoreApps.Interfaces;
using Windows.ApplicationModel.Search;

namespace Microsoft.Practices.Prism.StoreApps
{
    
    public class SearchPaneService : ISearchPaneService
    {
        public void Show()
        {
            SearchPane.GetForCurrentView().Show();
        }

        public void ShowOnKeyboardInput(bool enable)
        {
            SearchPane.GetForCurrentView().ShowOnKeyboardInput = enable;
        }

        public bool IsShowOnKeyboardInputEnabled()
        {
            if (!AppManifestHelper.IsSearchDeclared())
            {
                return false;
            }

            return SearchPane.GetForCurrentView().ShowOnKeyboardInput;
        }
    }
}
