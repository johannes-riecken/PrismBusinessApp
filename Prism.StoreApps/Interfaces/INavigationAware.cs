

using System.Collections.Generic;
using Windows.UI.Xaml.Navigation;

namespace Microsoft.Practices.Prism.StoreApps.Interfaces
{
    public interface INavigationAware
    {
        void OnNavigatedTo(object navigationParameter, NavigationMode navigationMode, Dictionary<string, object> viewModelState);
        
        void OnNavigatedFrom(Dictionary<string, object> viewModelState, bool suspending);
    }
}
