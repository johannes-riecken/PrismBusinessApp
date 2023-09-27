

using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using AdventureWorks.UILogic.Repositories;
using Microsoft.Practices.Prism.StoreApps;
using Microsoft.Practices.Prism.StoreApps.Interfaces;
using Windows.UI.Xaml.Navigation;
using AdventureWorks.UILogic.Services;
using System.Threading.Tasks;
using System;
using Windows.UI.Notifications;
using AdventureWorks.UILogic.Models;

namespace AdventureWorks.UILogic.ViewModels
{
    public class ItemDetailPageViewModel : ViewModel
    {
        private readonly IProductCatalogRepository _productCatalogRepository;
        private readonly IShoppingCartRepository _shoppingCartRepository;
        private readonly IAlertMessageService _alertService;
        private readonly IResourceLoader _resourceLoader;
        private readonly ISecondaryTileService _secondaryTileService;
        private readonly ISearchPaneService _searchPaneService;
        private IReadOnlyCollection<ProductViewModel> _items;
        private ProductViewModel _selectedProduct;
        private bool _isSelectedProductPinned;
        private string _title;
        private bool _isBottomAppBarSticky;
        private bool _isBottomAppBarOpened;
        private int _selectedIndex;

        public ItemDetailPageViewModel(IProductCatalogRepository productCatalogRepository, IShoppingCartRepository shoppingCartRepository, IAlertMessageService alertMessageService, IResourceLoader resourceLoader, ISecondaryTileService secondaryTileService, ISearchPaneService searchPaneService)
        {
            _productCatalogRepository = productCatalogRepository;
            _shoppingCartRepository = shoppingCartRepository;
            _alertService = alertMessageService;
            _resourceLoader = resourceLoader;
            _secondaryTileService = secondaryTileService;
            _searchPaneService = searchPaneService;

            PinProductCommand = DelegateCommand.FromAsyncHandler(PinProduct, () => SelectedProduct != null);
            UnpinProductCommand = DelegateCommand.FromAsyncHandler(UnpinProduct, () => SelectedProduct != null);
        }

        public DelegateCommand PinProductCommand { get; private set; }
        public DelegateCommand UnpinProductCommand { get; private set; }

        public ProductViewModel SelectedProduct
        {
            get { return _selectedProduct; }
            set
            {
                if (SetProperty(ref _selectedProduct, value) && value != null)
                {
                    IsSelectedProductPinned = _secondaryTileService.SecondaryTileExists(_selectedProduct.ProductNumber);
                }
            }
        }

        [RestorableState]
        public int SelectedIndex
        {
            get { return _selectedIndex; }
            set { SetProperty(ref _selectedIndex, value); }
        }

        public bool IsSelectedProductPinned
        {
            get { return _isSelectedProductPinned; }
            private set { SetProperty(ref _isSelectedProductPinned, value); }
        }

        public IReadOnlyCollection<ProductViewModel> Items
        {
            get { return _items; }
            private set { SetProperty(ref _items, value); }
        }

        public string Title
        {
            get { return _title; }
            private set { SetProperty(ref _title, value); }
        }

        public bool IsBottomAppBarSticky
        {
            get { return _isBottomAppBarSticky; }
            set { SetProperty(ref _isBottomAppBarSticky, value); }
        }

        public bool IsBottomAppBarOpened
        {
            get { return _isBottomAppBarOpened; }
            set
            {
                _isBottomAppBarOpened = value;
                OnPropertyChanged("IsBottomAppBarOpened");
            }
        }

        public async override void OnNavigatedTo(object navigationParameter, NavigationMode navigationMode, Dictionary<string, object> viewModelState)
        {
            string errorMessage = string.Empty;
            try
            {
                var productNumber = navigationParameter as string;
                var selectedProduct = await _productCatalogRepository.GetProductAsync(productNumber);
                var productViewModels = (await _productCatalogRepository.GetProductsAsync(selectedProduct.SubcategoryId))
                                                                        .Select(product => new ProductViewModel(product, _shoppingCartRepository, _alertService, _resourceLoader));

                var items = new ReadOnlyCollection<ProductViewModel>(productViewModels.ToList());
                Items = items;
                SelectedProduct = Items.First(p => p.ProductNumber == productNumber);
                SelectedIndex = items.IndexOf(SelectedProduct);
                Title = SelectedProduct.Title;
                _searchPaneService.ShowOnKeyboardInput(true);
            }
            catch (Exception ex)
            {
                errorMessage = string.Format(CultureInfo.CurrentCulture, _resourceLoader.GetString("GeneralServiceErrorMessage"), Environment.NewLine, ex.Message);
            }

            if (!string.IsNullOrWhiteSpace(errorMessage))
            {
                await _alertService.ShowAsync(errorMessage, _resourceLoader.GetString("ErrorServiceUnreachable"));
            }

            if (navigationMode != NavigationMode.New)
            {
                base.OnNavigatedTo(navigationParameter, navigationMode, viewModelState);
            }
        }

        public override void OnNavigatedFrom(Dictionary<string, object> viewModelState, bool suspending)
        {
            base.OnNavigatedFrom(viewModelState, suspending);
            if (!suspending)
            {
                _searchPaneService.ShowOnKeyboardInput(false);
            }
        }

        private async Task PinProduct()
        {
            if (SelectedProduct == null)
            {
                return;
            }

            var tileId = SelectedProduct.ProductNumber;

            bool isPinned = _secondaryTileService.SecondaryTileExists(tileId);

            if (!isPinned)
            {
                IsBottomAppBarSticky = true;

                isPinned = await _secondaryTileService.PinWideSecondaryTile(tileId, SelectedProduct.Title, SelectedProduct.ProductNumber);
                IsSelectedProductPinned = isPinned;

                IsBottomAppBarSticky = false;

                IsBottomAppBarOpened = false;

                if (IsSelectedProductPinned)
                {
                    var tileContentUri = new Uri(Constants.ServerAddress + "/api/TileNotification/" + tileId);
                    _secondaryTileService.ActivateTileNotifications(tileId, tileContentUri, PeriodicUpdateRecurrence.HalfHour);
                }
            }
        }

        private async Task UnpinProduct()
        {
            if (SelectedProduct == null)
            {
                return;
            }

            var tileId = SelectedProduct.ProductNumber;

            bool isPinned = _secondaryTileService.SecondaryTileExists(tileId);

            if (isPinned)
            {
                IsBottomAppBarSticky = true;

                isPinned = (await _secondaryTileService.UnpinTile(tileId)) == false;
                IsSelectedProductPinned = isPinned;

                IsBottomAppBarSticky = false;
                IsBottomAppBarOpened = false;
            }
        }
    }
}
