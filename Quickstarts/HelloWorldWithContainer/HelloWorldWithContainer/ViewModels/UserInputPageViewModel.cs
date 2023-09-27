

using HelloWorldWithContainer.Services;
using Microsoft.Practices.Prism.StoreApps;
using Microsoft.Practices.Prism.StoreApps.Interfaces;

namespace HelloWorldWithContainer.ViewModels
{

    public class UserInputPageViewModel : ViewModel
    {
        private readonly IDataRepository _dataRepository;
        private readonly INavigationService _navService;
        private string _VMState;

        public UserInputPageViewModel(IDataRepository dataRepository, INavigationService navService)
        {
            _navService = navService;
            _dataRepository = dataRepository;
            GoBackCommand = new DelegateCommand(_navService.GoBack);
        }

        public DelegateCommand GoBackCommand { get; set; }

        [RestorableState]
        public string VMState
        {
            get { return _VMState; }
            set { SetProperty(ref _VMState, value); }
        }

        public string ServiceState
        {
            get { return _dataRepository.GetUserEnteredData(); }
            set { _dataRepository.SetUserEnteredData(value); }
        }
    }
}
