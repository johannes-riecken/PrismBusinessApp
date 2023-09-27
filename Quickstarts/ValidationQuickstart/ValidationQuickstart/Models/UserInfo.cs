

using System.ComponentModel.DataAnnotations;
using Microsoft.Practices.Prism.StoreApps;

namespace ValidationQuickStart.Models
{

    public class UserInfo : ValidatableBindableBase
    {
        private string _firstName;
        private string _middleName;
        private string _lastName;

        private const string RegexPattern = @"\A\p{L}+([\p{Zs}\-][\p{L}]+)*\z";

        [Required(ErrorMessageResourceType = typeof(ErrorMessagesHelper), ErrorMessageResourceName = "FirstNameRequired")]
        [RegularExpression(RegexPattern, ErrorMessageResourceType = typeof(ErrorMessagesHelper), ErrorMessageResourceName = "FirstNameRegex")]
        public string FirstName
        {
            get { return _firstName; }
            set { SetProperty(ref _firstName, value); }
        }

        [Required(ErrorMessageResourceType = typeof(ErrorMessagesHelper), ErrorMessageResourceName = "MiddleNameRequired")]
        [RegularExpression(RegexPattern, ErrorMessageResourceType = typeof(ErrorMessagesHelper), ErrorMessageResourceName = "MiddleNameRegex")]
        public string MiddleName
        {
            get { return _middleName; }
            set { SetProperty(ref _middleName, value); }
        }

        [Required(ErrorMessageResourceType = typeof(ErrorMessagesHelper), ErrorMessageResourceName = "LastNameRequired")]
        [RegularExpression(RegexPattern, ErrorMessageResourceType = typeof(ErrorMessagesHelper), ErrorMessageResourceName = "LastNameRegex")]
        public string LastName
        {
            get { return _lastName; }
            set { SetProperty(ref _lastName, value); }
        }
    }
}
