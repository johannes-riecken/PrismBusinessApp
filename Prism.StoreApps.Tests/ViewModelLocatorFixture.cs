

using System;
using System.Threading.Tasks;
using Microsoft.Practices.Prism.StoreApps.Tests.Mocks;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using Windows.ApplicationModel.Core;
using Windows.Foundation;
using Windows.UI.Core;

namespace Microsoft.Practices.Prism.StoreApps.Tests
{
    [TestClass]
    public class ViewModelLocatorFixture
    {
        public IAsyncAction ExecuteOnUIThread(DispatchedHandler action)
        {
            return CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, action);
        } 

        [TestMethod]
        public async Task AutoWireViewModel_With_Factory_Registration()
        {
            await ExecuteOnUIThread(() =>
            {
                var page = new MockPage();

                ViewModelLocator.Register(typeof(MockPage).ToString(), () => new MockPageViewModel());

                ViewModelLocator.SetAutoWireViewModel(page, true);

                Assert.IsNotNull(page.DataContext);
                Assert.IsInstanceOfType(page.DataContext, typeof(MockPageViewModel));
            });
        }

        [TestMethod]
        public async Task AutoWireViewModel_With_Custom_Resolver()
        {
            await ExecuteOnUIThread(() =>
            {
                var page = new MockPage();

                ViewModelLocator.SetDefaultViewTypeToViewModelTypeResolver((viewType) =>
                {
                    var viewName = viewType.FullName;

                    var viewModelName = String.Format("{0}ViewModel", viewName);
                    return Type.GetType(viewModelName);
                });

                ViewModelLocator.SetAutoWireViewModel(page, true);

                Assert.IsNotNull(page.DataContext);
                Assert.IsInstanceOfType(page.DataContext, typeof(MockPageViewModel));
            });
        }

        [TestMethod]
        public async Task AutoWireViewModel_With_Custom_Resolver_And_Factory()
        {
            await ExecuteOnUIThread(() =>
            {
                var page = new MockPage();

                ViewModelLocator.SetDefaultViewTypeToViewModelTypeResolver((viewType) =>
                {
                    var viewName = viewType.FullName;

                    var viewModelName = String.Format("{0}ViewModel", viewName);
                    return Type.GetType(viewModelName);
                });

                ViewModelLocator.SetDefaultViewModelFactory((viewModelType) =>
                {
                    return Activator.CreateInstance(viewModelType) as ViewModel;
                });

                ViewModelLocator.SetAutoWireViewModel(page, true);

                Assert.IsNotNull(page.DataContext);
                Assert.IsInstanceOfType(page.DataContext, typeof(MockPageViewModel));
            });
        }
    }
}
