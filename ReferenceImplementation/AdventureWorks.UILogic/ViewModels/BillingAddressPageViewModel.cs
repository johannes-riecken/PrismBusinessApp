

using System;
using System.Windows.Input;
using AdventureWorks.UILogic.Models;
using Microsoft.Practices.Prism.StoreApps;
using Microsoft.Practices.Prism.StoreApps.Interfaces;
using AdventureWorks.UILogic.Repositories;
using AdventureWorks.UILogic.Services;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;

namespace AdventureWorks.UILogic.ViewModels
{
    public class BillingAddressPageViewModel : ViewModel
    {
        private readonly IBillingAddressUserControlViewModel _billingAddressViewModel;
        private readonly IResourceLoader _resourceLoader;
        private readonly IAlertMessageService _alertMessageService;
        private readonly IAccountService _accountService;
        private readonly INavigationService _navigationService;
        private string _headerLabel;

        public BillingAddressPageViewModel(IBillingAddressUserControlViewModel billingAddressViewModel, IResourceLoader resourceLoader, IAlertMessageService alertMessageService, IAccountService accountService, INavigationService navigationService)
        {
            _billingAddressViewModel = billingAddressViewModel;
            _resourceLoader = resourceLoader;
            _alertMessageService = alertMessageService;
            _accountService = accountService;
            _navigationService = navigationService;

            SaveCommand = DelegateCommand.FromAsyncHandler(SaveAsync);
        }

        public IBillingAddressUserControlViewModel BillingAddressViewModel
        {
            get { return _billingAddressViewModel; } 
        }

        public string HeaderLabel
        {
            get { return _headerLabel; }
            private set { SetProperty(ref _headerLabel, value); }
        }

        public ICommand SaveCommand { get; private set; }

        public override async void OnNavigatedTo(object navigationParameter, Windows.UI.Xaml.Navigation.NavigationMode navigationMode, Dictionary<string, object> viewModelState)
        {
            if (await _accountService.VerifyUserAuthenticationAsync() == null) return;

            var addressId = navigationParameter as string;

            HeaderLabel = string.IsNullOrWhiteSpace(addressId)
                              ? _resourceLoader.GetString("AddBillingAddressTitle")
                              : _resourceLoader.GetString("EditBillingAddressTitle");

            BillingAddressViewModel.OnNavigatedTo(navigationParameter, navigationMode, viewModelState);
        }

        public override void OnNavigatedFrom(Dictionary<string, object> viewModelState, bool suspending)
        {
            BillingAddressViewModel.OnNavigatedFrom(viewModelState, suspending);
        }

        private async Task SaveAsync()
        {
            if (BillingAddressViewModel.ValidateForm())
            {
                string errorMessage = string.Empty;

                try
                {
                    await BillingAddressViewModel.ProcessFormAsync();
                    _navigationService.GoBack();
                }
                catch (ModelValidationException mvex)
                {
                    DisplayValidationErrors(mvex.ValidationResult);
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
        }

        private void DisplayValidationErrors(ModelValidationResult modelValidationResults)
        {
            var errors = new Dictionary<string, ReadOnlyCollection<string>>();

            foreach (var propkey in modelValidationResults.ModelState.Keys)
            {
                string propertyName = propkey.Substring(propkey.IndexOf('.') + 1); // strip off order. prefix

                errors.Add(propertyName, new ReadOnlyCollection<string>(modelValidationResults.ModelState[propkey]));
            }

            if (errors.Count > 0) BillingAddressViewModel.Address.Errors.SetAllErrors(errors);
        }
    }
}
