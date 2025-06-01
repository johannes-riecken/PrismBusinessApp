

using System.Collections.Generic;

namespace Microsoft.Practices.Prism.StoreApps.Interfaces
{
    public interface INavigationAware
    {
        void OnNavigatedTo(object navigationParameter, string navigationMode, Dictionary<string, object> viewModelState);
        
        void OnNavigatedFrom(Dictionary<string, object> viewModelState, bool suspending);
    }
}
