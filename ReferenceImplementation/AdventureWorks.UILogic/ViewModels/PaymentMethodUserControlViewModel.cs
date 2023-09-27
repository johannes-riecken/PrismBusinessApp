

using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using AdventureWorks.UILogic.Models;
using AdventureWorks.UILogic.Repositories;
using Microsoft.Practices.Prism.StoreApps;
using Windows.UI.Xaml.Navigation;
using System.Threading.Tasks;

namespace AdventureWorks.UILogic.ViewModels
{
    public class PaymentMethodUserControlViewModel : ViewModel, IPaymentMethodUserControlViewModel
    {
        private bool _loadDefault;
        private readonly ICheckoutDataRepository _checkoutDataRepository;
        private PaymentMethod _paymentMethod;

        public PaymentMethodUserControlViewModel(ICheckoutDataRepository checkoutDataRepository)
        {
            _paymentMethod = new PaymentMethod();
            _checkoutDataRepository = checkoutDataRepository;
        }

        [RestorableState]
        public PaymentMethod PaymentMethod
        {
            get { return _paymentMethod; }
            set { SetProperty(ref _paymentMethod, value); }
        }

        public override async void OnNavigatedTo(object navigationParameter, NavigationMode navigationMode, Dictionary<string, object> viewState)
        {
            if (viewState != null)
            {
                base.OnNavigatedTo(navigationParameter, navigationMode, viewState);

                if (navigationMode == NavigationMode.Refresh)
                {
                    var errorsCollection = RetrieveEntityStateValue<IDictionary<string, ReadOnlyCollection<string>>>("errorsCollection", viewState);

                    if (errorsCollection != null)
                    {
                        _paymentMethod.SetAllErrors(errorsCollection);
                    }
                }
            }

            if (navigationMode == NavigationMode.New)
            {
                if (_loadDefault)
                {
                    var defaultPaymentMethod = await _checkoutDataRepository.GetDefaultPaymentMethodAsync();
                    if (defaultPaymentMethod != null)
                    {
                        PaymentMethod.CardNumber = defaultPaymentMethod.CardNumber;
                        PaymentMethod.CardVerificationCode = defaultPaymentMethod.CardVerificationCode;
                        PaymentMethod.CardholderName = defaultPaymentMethod.CardholderName;
                        PaymentMethod.ExpirationMonth = defaultPaymentMethod.ExpirationMonth;
                        PaymentMethod.ExpirationYear = defaultPaymentMethod.ExpirationYear;
                        PaymentMethod.Phone = defaultPaymentMethod.Phone;

                        ValidateForm();
                    }
                }
            }
        }

        public override void OnNavigatedFrom(Dictionary<string, object> viewState, bool suspending)
        {
            base.OnNavigatedFrom(viewState, suspending);

            if (viewState != null)
            {
                AddEntityStateValue("errorsCollection", _paymentMethod.GetAllErrors(), viewState);
            }
        }

        public async Task ProcessFormAsync()
        {
            var existingPaymentMethods = await _checkoutDataRepository.GetAllPaymentMethodsAsync();
            var matchingExistingPaymentMethod = FindMatchingPaymentMethod(PaymentMethod, existingPaymentMethods);
            if (matchingExistingPaymentMethod != null)
            {
                PaymentMethod = matchingExistingPaymentMethod;
            }
            else
            {
                await _checkoutDataRepository.SavePaymentMethodAsync(PaymentMethod);
            }
        }

        public bool ValidateForm()
        {
            return _paymentMethod.ValidateProperties();
        }

        public void SetLoadDefault(bool loadDefault)
        {
            _loadDefault = loadDefault;
        }

        private static PaymentMethod FindMatchingPaymentMethod(PaymentMethod searchPaymentMethod, IEnumerable<PaymentMethod> paymentMethods)
        {
            return paymentMethods.FirstOrDefault(paymentMethod =>
                searchPaymentMethod.CardVerificationCode == paymentMethod.CardVerificationCode &&
                searchPaymentMethod.CardholderName == paymentMethod.CardholderName &&
                searchPaymentMethod.ExpirationMonth == paymentMethod.ExpirationMonth &&
                searchPaymentMethod.ExpirationYear == paymentMethod.ExpirationYear &&
                searchPaymentMethod.Phone == paymentMethod.Phone);
        }
    }
}
