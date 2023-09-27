

using Microsoft.Practices.Prism.StoreApps.Interfaces;

namespace AdventureWorks.UILogic.Tests.Mocks
{
    public class MockSearchPaneService : ISearchPaneService
    {
        public void Show()
        {
        }

        public void ShowOnKeyboardInput(bool enable)
        {
        }

        public bool IsShowOnKeyboardInputEnabled()
        {
            return true;
        }
    }
}
