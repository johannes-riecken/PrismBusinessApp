

using Windows.UI.Xaml.Controls;

namespace Microsoft.Practices.Prism.StoreApps.Tests.Mocks
{
    public class MockPageWithViewModel : Page
    {
        public MockPageWithViewModel()
        {
            var viewModel = new MockViewModelWithRestorableStateAttributes();
            viewModel.Title = "testtitle";
            this.DataContext = viewModel;
        }
    }
}
