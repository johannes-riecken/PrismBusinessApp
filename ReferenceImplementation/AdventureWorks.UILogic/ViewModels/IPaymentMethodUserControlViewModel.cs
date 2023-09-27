

using System.Collections.Generic;
using System.ComponentModel;
using AdventureWorks.UILogic.Models;
using Microsoft.Practices.Prism.StoreApps;
using Windows.UI.Xaml.Navigation;
using System.Threading.Tasks;

namespace AdventureWorks.UILogic.ViewModels
{
    public interface IPaymentMethodUserControlViewModel
    {
        [RestorableState]
        PaymentMethod PaymentMethod { get; set; }
        void OnNavigatedTo(object navigationParameter, NavigationMode navigationMode, Dictionary<string, object> viewState);
        void OnNavigatedFrom(Dictionary<string, object> viewState, bool suspending);
        Task ProcessFormAsync();
        bool ValidateForm();
        event PropertyChangedEventHandler PropertyChanged;
        void SetLoadDefault(bool loadDefault);
    }
}