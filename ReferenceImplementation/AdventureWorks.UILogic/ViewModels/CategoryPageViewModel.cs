

using System.Globalization;
using AdventureWorks.UILogic.Models;
using AdventureWorks.UILogic.Repositories;
using AdventureWorks.UILogic.Services;
using Microsoft.Practices.Prism.StoreApps;
using Microsoft.Practices.Prism.StoreApps.Interfaces;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Windows.UI.Xaml.Navigation;

namespace AdventureWorks.UILogic.ViewModels
{
    public class CategoryPageViewModel : ViewModel
    {
        private IProductCatalogRepository _productCatalogRepository;
        private INavigationService _navigationService;
        private readonly IAlertMessageService _alertMessageService;
        private readonly IResourceLoader _resourceLoader;
        private readonly ISearchPaneService _searchPaneService;
        private IReadOnlyCollection<CategoryViewModel> _subcategories;
        private string _title;

        public CategoryPageViewModel(IProductCatalogRepository productCatalogRepository, INavigationService navigationService, IAlertMessageService alertMessageService, IResourceLoader resourceLoader, ISearchPaneService searchPaneService)
        {
            _productCatalogRepository = productCatalogRepository;
            _navigationService = navigationService;
            _alertMessageService = alertMessageService;
            _resourceLoader = resourceLoader;
            _searchPaneService = searchPaneService;
        }

        public IReadOnlyCollection<CategoryViewModel> Subcategories
        {
            get { return _subcategories; }
            private set { SetProperty(ref _subcategories, value); }
        }

        [RestorableState]
        public string Title
        {
            get { return _title; }
            private set { SetProperty(ref _title, value); }
        }

        public async override void OnNavigatedTo(object navigationParameter, NavigationMode navigationMode, Dictionary<string, object> viewModelState)
        {
            base.OnNavigatedTo(navigationParameter, navigationMode, viewModelState);
            int parentCategoryId = int.Parse(navigationParameter.ToString());
            ReadOnlyCollection<Category> subCategories = null;
            string errorMessage = string.Empty;
            try
            {
                subCategories = await _productCatalogRepository.GetSubcategoriesAsync(parentCategoryId, 5);
            }
            catch (Exception ex)
            {
                errorMessage = string.Format(CultureInfo.CurrentCulture, _resourceLoader.GetString("GeneralServiceErrorMessage"), Environment.NewLine, ex.Message);

            }

            if (!string.IsNullOrWhiteSpace(errorMessage))
            {
                await _alertMessageService.ShowAsync(errorMessage, _resourceLoader.GetString("ErrorServiceUnreachable"));
                return;
            }

            if (string.IsNullOrEmpty(Title))
            {
                Title = _productCatalogRepository.GetCategoryName(parentCategoryId);
            }

            var subCategoryViewModels = new List<CategoryViewModel>();
            foreach (var subCategory in subCategories)
            {
                subCategoryViewModels.Add(new CategoryViewModel(subCategory, _navigationService));
            }
            Subcategories = new ReadOnlyCollection<CategoryViewModel>(subCategoryViewModels);
            _searchPaneService.ShowOnKeyboardInput(true);
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
