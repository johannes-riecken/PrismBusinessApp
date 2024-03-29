

using System.Globalization;
using AdventureWorks.UILogic.Models;
using AdventureWorks.UILogic.Repositories;
using AdventureWorks.UILogic.Services;
using AdventureWorks.UILogic.ViewModels;
using Microsoft.Practices.Prism.PubSubEvents;
using Microsoft.Practices.Prism.StoreApps;
using Microsoft.Practices.Prism.StoreApps.Interfaces;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Windows.ApplicationModel.Activation;
using Windows.ApplicationModel.Resources;
using Windows.ApplicationModel.Search;
using Windows.System;
using Windows.UI.Notifications;
using Windows.UI.Xaml;
using AdventureWorks.Shopper.Services;
using AdventureWorks.Shopper.Views;
using Windows.UI.ApplicationSettings;

namespace AdventureWorks.Shopper
{
    sealed partial class App : MvvmAppBase
    {
        private readonly IUnityContainer _container = new UnityContainer();

        private IEventAggregator _eventAggregator;
        private TileUpdater _tileUpdater;

        public App()
        {
            this.InitializeComponent();
            this.RequestedTheme = ApplicationTheme.Dark;
        }

        protected override void OnLaunchApplication(LaunchActivatedEventArgs args)
        {
            if (args != null && !string.IsNullOrEmpty(args.Arguments))
            {
                NavigationService.Navigate("ItemDetail", args.Arguments);
            }
            else
            {
                NavigationService.Navigate("Hub", null);
            }
        }

        protected override void OnSearchApplication(SearchQueryArguments args)
        {
            if (args != null && !string.IsNullOrEmpty(args.QueryText))
            {
                NavigationService.Navigate("SearchResults", args.QueryText);
            }
            else
            {
                NavigationService.Navigate("Hub", null);
            }
        }

        protected override void OnRegisterKnownTypesForSerialization()
        {
            SessionStateService.RegisterKnownType(typeof(Address));
            SessionStateService.RegisterKnownType(typeof(PaymentMethod));
            SessionStateService.RegisterKnownType(typeof(UserInfo));
            SessionStateService.RegisterKnownType(typeof(CheckoutDataViewModel));
            SessionStateService.RegisterKnownType(typeof(ObservableCollection<CheckoutDataViewModel>));
            SessionStateService.RegisterKnownType(typeof(ShippingMethod));
            SessionStateService.RegisterKnownType(typeof(ReadOnlyDictionary<string, ReadOnlyCollection<string>>));
            SessionStateService.RegisterKnownType(typeof(Order));
            SessionStateService.RegisterKnownType(typeof(Product));
            SessionStateService.RegisterKnownType(typeof(ReadOnlyCollection<Product>));
        }

        protected override void OnInitialize(IActivatedEventArgs args)
        {
            _eventAggregator = new EventAggregator();

            _container.RegisterInstance<INavigationService>(NavigationService);
            _container.RegisterInstance<ISessionStateService>(SessionStateService);
            _container.RegisterInstance<IEventAggregator>(_eventAggregator);
            _container.RegisterInstance<IResourceLoader>(new ResourceLoaderAdapter(new ResourceLoader()));

            _container.RegisterType<IAccountService, AccountService>(new ContainerControlledLifetimeManager());
            _container.RegisterType<ICredentialStore, RoamingCredentialStore>(new ContainerControlledLifetimeManager());
            _container.RegisterType<ICacheService, TemporaryFolderCacheService>(new ContainerControlledLifetimeManager());
            _container.RegisterType<ISecondaryTileService, SecondaryTileService>(new ContainerControlledLifetimeManager());
            _container.RegisterType<IAlertMessageService, AlertMessageService>(new ContainerControlledLifetimeManager());
            _container.RegisterType<ISearchPaneService, SearchPaneService>(new ContainerControlledLifetimeManager());

            _container.RegisterType<IProductCatalogRepository, ProductCatalogRepository>(new ContainerControlledLifetimeManager());
            _container.RegisterType<IShoppingCartRepository, ShoppingCartRepository>(new ContainerControlledLifetimeManager());
            _container.RegisterType<ICheckoutDataRepository, CheckoutDataRepository>(new ContainerControlledLifetimeManager());
            _container.RegisterType<IOrderRepository, OrderRepository>(new ContainerControlledLifetimeManager());

            _container.RegisterType<IProductCatalogService, ProductCatalogServiceProxy>(new ContainerControlledLifetimeManager());
            _container.RegisterType<IOrderService, OrderServiceProxy>(new ContainerControlledLifetimeManager());
            _container.RegisterType<IShoppingCartService, ShoppingCartServiceProxy>(new ContainerControlledLifetimeManager());
            _container.RegisterType<IShippingMethodService, ShippingMethodServiceProxy>(new ContainerControlledLifetimeManager());
            _container.RegisterType<IIdentityService, IdentityServiceProxy>(new ContainerControlledLifetimeManager());
            _container.RegisterType<ILocationService, LocationServiceProxy>(new ContainerControlledLifetimeManager());
            _container.RegisterType<IAddressService, AddressServiceProxy>(new ContainerControlledLifetimeManager());
            _container.RegisterType<IPaymentMethodService, PaymentMethodServiceProxy>(new ContainerControlledLifetimeManager());

            _container.RegisterType<IShippingAddressUserControlViewModel, ShippingAddressUserControlViewModel>();
            _container.RegisterType<IBillingAddressUserControlViewModel, BillingAddressUserControlViewModel>();
            _container.RegisterType<IPaymentMethodUserControlViewModel, PaymentMethodUserControlViewModel>();
            _container.RegisterType<ISignInUserControlViewModel, SignInUserControlViewModel>();

            ViewModelLocator.SetDefaultViewTypeToViewModelTypeResolver((viewType) =>
                {
                    var viewModelTypeName = string.Format(CultureInfo.InvariantCulture, "AdventureWorks.UILogic.ViewModels.{0}ViewModel, AdventureWorks.UILogic, Version=1.0.0.0, Culture=neutral, PublicKeyToken=634ac3171ee5190a", viewType.Name);
                    var viewModelType = Type.GetType(viewModelTypeName);
                    return viewModelType;
                });

            _tileUpdater = TileUpdateManager.CreateTileUpdaterForApplication();
            _tileUpdater.StartPeriodicUpdate(new Uri(Constants.ServerAddress + "/api/TileNotification"), PeriodicUpdateRecurrence.HalfHour);

            var resourceLoader = _container.Resolve<IResourceLoader>();
            SearchPane.GetForCurrentView().PlaceholderText = resourceLoader.GetString("SearchPanePlaceHolderText");
        }

        protected override object Resolve(Type type)
        {
            return _container.Resolve(type);
        }

        protected override IList<SettingsCommand> GetSettingsCommands()
        {
            var settingsCommands = new List<SettingsCommand>();
            var accountService = _container.Resolve<IAccountService>();
            var resourceLoader = _container.Resolve<IResourceLoader>();

            if (accountService.SignedInUser == null)
            {
                settingsCommands.Add(new SettingsCommand(Guid.NewGuid().ToString(), resourceLoader.GetString("LoginText"), (c) => new SignInFlyout().Show()));
            }
            else
            {
                settingsCommands.Add(new SettingsCommand(Guid.NewGuid().ToString(), resourceLoader.GetString("LogoutText"), (c) => new SignOutFlyout().Show()));
                settingsCommands.Add(new SettingsCommand(Guid.NewGuid().ToString(), resourceLoader.GetString("AddShippingAddressTitle"), (c) => NavigationService.Navigate("ShippingAddress", null)));
                settingsCommands.Add(new SettingsCommand(Guid.NewGuid().ToString(), resourceLoader.GetString("AddBillingAddressTitle"), (c) => NavigationService.Navigate("BillingAddress", null)));
                settingsCommands.Add(new SettingsCommand(Guid.NewGuid().ToString(), resourceLoader.GetString("AddPaymentMethodTitle"), (c) => NavigationService.Navigate("PaymentMethod", null)));
                settingsCommands.Add(new SettingsCommand(Guid.NewGuid().ToString(), resourceLoader.GetString("ChangeDefaults"), (c) => new ChangeDefaultsFlyout().Show()));
            }
            settingsCommands.Add(new SettingsCommand(Guid.NewGuid().ToString(), resourceLoader.GetString("PrivacyPolicy"), async (c) => await Launcher.LaunchUriAsync(new Uri(resourceLoader.GetString("PrivacyPolicyUrl")))));

            return settingsCommands;
        }
    }
}
