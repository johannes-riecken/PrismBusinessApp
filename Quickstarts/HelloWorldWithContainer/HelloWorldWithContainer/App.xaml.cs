

using HelloWorldWithContainer.Services;
using Microsoft.Practices.Prism.StoreApps;
using Microsoft.Practices.Prism.StoreApps.Interfaces;
using Microsoft.Practices.Unity;
using Windows.ApplicationModel.Activation;



namespace HelloWorldWithContainer
{
    sealed partial class App : MvvmAppBase
    {
        IUnityContainer _container = new UnityContainer();

        public App()
        {
            InitializeComponent();
        }

        protected override void OnLaunchApplication(LaunchActivatedEventArgs args)
        {
            NavigationService.Navigate("Main", null);
        }

        protected override void OnInitialize(IActivatedEventArgs args)
        {
            _container.RegisterInstance<ISessionStateService>(SessionStateService);
            _container.RegisterInstance<INavigationService>(NavigationService);
            _container.RegisterType<IDataRepository, DataRepository>();

            ViewModelLocator.SetDefaultViewModelFactory((viewModelType) => _container.Resolve(viewModelType));
        }
    }
}
