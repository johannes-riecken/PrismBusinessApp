

using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows.Input;
using Microsoft.Practices.Prism.StoreApps;
using ValidationQuickStart.Models;

namespace ValidationQuickStart.ViewModels
{

    public class UserInfoViewModel : ViewModel
    {
        private UserInfo _userInfo;
        private ReadOnlyCollection<string> _allErrors;

        public UserInfoViewModel()
            : this(new UserInfo())
        {
        }

        public UserInfoViewModel(UserInfo userInfo)
        {
            _userInfo = userInfo;
            if (_userInfo != null)
            {
                _userInfo.ErrorsChanged += OnErrorsChanged;
            }
            _allErrors = BindableValidator.EmptyErrorsCollection;
            ValidateCommand = new DelegateCommand(Validate);
        }

        public UserInfo UserInfo
        {
            get { return _userInfo; }
            set { SetProperty(ref _userInfo, value); }
        }

        public ReadOnlyCollection<string> AllErrors
        {
            get { return _allErrors; }
            private set { SetProperty(ref _allErrors, value); }
        }

        public ICommand ValidateCommand { get; set; }

        private void Validate()
        {
            _userInfo.ValidateProperties();
        }

        private void OnErrorsChanged(object sender, DataErrorsChangedEventArgs e)
        {
            AllErrors = new ReadOnlyCollection<string>(_userInfo.GetAllErrors().Values.SelectMany(c => c).ToList());
        }
    }
}
