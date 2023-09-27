

namespace Microsoft.Practices.Prism.StoreApps.Interfaces
{
    public interface ISearchPaneService
    {
        void Show();

        void ShowOnKeyboardInput(bool enable);

        bool IsShowOnKeyboardInputEnabled();
    }
}