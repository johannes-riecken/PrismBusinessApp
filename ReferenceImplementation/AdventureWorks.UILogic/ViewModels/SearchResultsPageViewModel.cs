

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Windows.Input;
using AdventureWorks.UILogic.Models;
using AdventureWorks.UILogic.Repositories;
using AdventureWorks.UILogic.Services;
using Microsoft.Practices.Prism.StoreApps;
using Microsoft.Practices.Prism.StoreApps.Interfaces;
using Windows.UI.Xaml.Navigation;

namespace AdventureWorks.UILogic.ViewModels
{

    public class SearchResultsPageViewModel : ViewModel
    {
        private readonly IProductCatalogRepository _productCatalogRepository;
        private readonly ISearchPaneService _searchPaneService;
        private readonly IResourceLoader _resourceLoader;
        private readonly IAlertMessageService _alertMessageService;
        private string _searchTerm;
        private string _queryString;
        private bool _noResults;
        private ReadOnlyCollection<ProductViewModel> _results;
        private int _totalCount;

        public SearchResultsPageViewModel(IProductCatalogRepository productCatalogRepository, ISearchPaneService searchPaneService, IResourceLoader resourceLoader, IAlertMessageService alertMessageService)
        {
            _productCatalogRepository = productCatalogRepository;
            _searchPaneService = searchPaneService;
            _resourceLoader = resourceLoader;
            _alertMessageService = alertMessageService;
        }

        [RestorableState]
        public static string PreviousSearchTerm { get; private set; }

        [RestorableState]
        public static ReadOnlyCollection<Product> PreviousResults { get; private set; }

        public string QueryText
        {
            get { return _queryString; }
            private set { SetProperty(ref this._queryString, value); }
        }

        public string SearchTerm
        {
            get { return _searchTerm; }
            private set { SetProperty(ref this._searchTerm, value); }
        }

        public ReadOnlyCollection<ProductViewModel> Results
        {
            get { return _results; }
            private set { SetProperty(ref _results, value); }
        }

        [RestorableState]
        public int TotalCount
        {
            get { return _totalCount; }
            private set { SetProperty(ref _totalCount, value); }
        }

        public bool NoResults
        {
            get { return _noResults; }
            private set { SetProperty(ref _noResults, value); }
        }

        public async override void OnNavigatedTo(object navigationParameter, NavigationMode navigationMode, Dictionary<string, object> viewModelState)
        {
            base.OnNavigatedTo(navigationParameter, navigationMode, viewModelState);
            var queryText = navigationParameter as String;
            string errorMessage = string.Empty;
            this.SearchTerm = queryText;
            this.QueryText = '\u201c' + queryText + '\u201d';

            try
            {
                ReadOnlyCollection<Product> products;
                if (queryText == PreviousSearchTerm)
                {
                    products = PreviousResults;
                }
                else
                {
                    var searchResults = await _productCatalogRepository.GetFilteredProductsAsync(queryText);
                    products = searchResults.Products;
                    TotalCount = searchResults.TotalCount;
                    PreviousResults = products;
                }


                var productViewModels = new List<ProductViewModel>();
                foreach (var product in products)
                {
                    productViewModels.Add(new ProductViewModel(product));
                }

                this.Results = new ReadOnlyCollection<ProductViewModel>(productViewModels);
                this.NoResults = !this.Results.Any();

                PreviousSearchTerm = SearchTerm;
                _searchPaneService.ShowOnKeyboardInput(true);
            }
            catch (Exception ex)
            {
                errorMessage = string.Format(CultureInfo.CurrentCulture, _resourceLoader.GetString("GeneralServiceErrorMessage"), Environment.NewLine, ex.Message);
            }

            if (!string.IsNullOrWhiteSpace(errorMessage))
            {
                await _alertMessageService.ShowAsync(errorMessage, _resourceLoader.GetString("ErrorServiceUnreachable"));
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

    }
}