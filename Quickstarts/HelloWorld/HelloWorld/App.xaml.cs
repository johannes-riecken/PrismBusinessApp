

using HelloWorld.Services;
using HelloWorld.ViewModels;
using HelloWorld.Views;
using Microsoft.Practices.Prism.StoreApps;
using Windows.ApplicationModel.Activation;



namespace HelloWorld
{
    sealed partial class App : MvvmAppBase
    {
        IDataRepository _dataRepository;
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
            _dataRepository = new DataRepository(SessionStateService);

            ViewModelLocator.Register(typeof(MainPage).ToString(), () => new MainPageViewModel(_dataRepository, NavigationService));
            ViewModelLocator.Register(typeof(UserInputPage).ToString(), () => new UserInputPageViewModel(_dataRepository, NavigationService));
        }
    }
}
