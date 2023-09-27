

namespace Microsoft.Practices.Prism.StoreApps.Interfaces
{
    public interface INavigationService
    {
        bool Navigate(string pageToken, object parameter);

        void GoBack();

        bool CanGoBack();

        void ClearHistory();

        void RestoreSavedNavigation();

        void Suspending();
    }
}
