

using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace Microsoft.Practices.Prism.StoreApps.Tests.Mocks
{
    public class MockPage : Page
    {
        public object PageParameter { get; set; }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (e != null)
            {
                this.PageParameter = e.Parameter;
            }
        }
    }
}
