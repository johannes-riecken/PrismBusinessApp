

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using AdventureWorks.UILogic.Services;
using Microsoft.Practices.Prism.StoreApps;

namespace AdventureWorks.UILogic.Models
{

    public class Address : ValidatableBindableBase
    {
        private string _id;
        private string _firstName;
        private string _middleInitial;
        private string _lastName;
        private string _streetAddress;
        private string _optionalAddress;
        private string _city;
        private string _state;
        private string _zipCode;
        private string _phone;

        
        private const string NAMES_REGEX_PATTERN = @"\A\p{L}+([\p{Zs}\-][\p{L}]+)*\z";

        private const string ADDRESS_REGEX_PATTERN = @"\A[\p{L}\p{N}]+([\p{Zs}][\p{L}\p{N}]+)*\z";

        private const string NUMBERS_REGEX_PATTERN = @"\A\p{N}+([\p{N}\-][\p{N}]+)*\z";

        public string Id
        {
            get { return _id; }
            set { SetProperty(ref _id, value); }
        }

        [Required(ErrorMessageResourceType = typeof(ErrorMessagesHelper), ErrorMessageResourceName = "RequiredErrorMessage")]
        [RegularExpression(NAMES_REGEX_PATTERN, ErrorMessageResourceType = typeof(ErrorMessagesHelper), ErrorMessageResourceName = "RegexErrorMessage")]
        public string FirstName
        {
            get { return _firstName; }
            set { SetProperty(ref _firstName, value); }
        }

        [RegularExpression(NAMES_REGEX_PATTERN, ErrorMessageResourceType = typeof(ErrorMessagesHelper), ErrorMessageResourceName = "RegexErrorMessage")]
        public string MiddleInitial
        {
            get { return _middleInitial; }
            set { SetProperty(ref _middleInitial, value); }
        }

        [Required(ErrorMessageResourceType = typeof(ErrorMessagesHelper), ErrorMessageResourceName = "RequiredErrorMessage")]
        [RegularExpression(NAMES_REGEX_PATTERN, ErrorMessageResourceType = typeof(ErrorMessagesHelper), ErrorMessageResourceName = "RegexErrorMessage")]
        public string LastName
        {
            get { return _lastName; }
            set { SetProperty(ref _lastName, value); }
        }

        [Required(ErrorMessageResourceType = typeof(ErrorMessagesHelper), ErrorMessageResourceName = "RequiredErrorMessage")]
        [RegularExpression(ADDRESS_REGEX_PATTERN, ErrorMessageResourceType = typeof(ErrorMessagesHelper), ErrorMessageResourceName = "RegexErrorMessage")]
        public string StreetAddress
        {
            get { return _streetAddress; }
            set { SetProperty(ref _streetAddress, value); }
        }

        [RegularExpression(ADDRESS_REGEX_PATTERN, ErrorMessageResourceType = typeof(ErrorMessagesHelper), ErrorMessageResourceName = "RegexErrorMessage")]
        public string OptionalAddress
        {
            get { return _optionalAddress; }
            set { SetProperty(ref _optionalAddress, value); }
        }

        [Required(ErrorMessageResourceType = typeof(ErrorMessagesHelper), ErrorMessageResourceName = "RequiredErrorMessage")]
        [RegularExpression(ADDRESS_REGEX_PATTERN, ErrorMessageResourceType = typeof(ErrorMessagesHelper), ErrorMessageResourceName = "RegexErrorMessage")]
        public string City
        {
            get { return _city; }
            set { SetProperty(ref _city, value); }
        }

        [Required(ErrorMessageResourceType = typeof(ErrorMessagesHelper), ErrorMessageResourceName = "RequiredErrorMessage")]
        [RegularExpression(ADDRESS_REGEX_PATTERN, ErrorMessageResourceType = typeof(ErrorMessagesHelper), ErrorMessageResourceName = "RegexErrorMessage")]
        public string State
        {
            get { return _state; }
            set { SetProperty(ref _state, value); }
        }

        [Required(ErrorMessageResourceType = typeof(ErrorMessagesHelper), ErrorMessageResourceName = "RequiredErrorMessage")]
        [RegularExpression(NUMBERS_REGEX_PATTERN, ErrorMessageResourceType = typeof(ErrorMessagesHelper), ErrorMessageResourceName = "ZipCodeRegexErrorMessage")]
        [StringLength(6, MinimumLength=3, ErrorMessageResourceType = typeof(ErrorMessagesHelper), ErrorMessageResourceName = "ZipCodeLengthInvalidErrorMessage")]
        public string ZipCode
        {
            get { return _zipCode; }
            set { SetProperty(ref _zipCode, value); }
        }

        [Required(ErrorMessageResourceType = typeof(ErrorMessagesHelper), ErrorMessageResourceName = "RequiredErrorMessage")]
        [RegularExpression(NUMBERS_REGEX_PATTERN, ErrorMessageResourceType = typeof(ErrorMessagesHelper), ErrorMessageResourceName = "RegexErrorMessage")]
        public string Phone
        {
            get { return _phone; }
            set { SetProperty(ref _phone, value); }
        }

        public AddressType AddressType { get; set; }

        public bool IsDefault { get; set; }

        public static Address FindMatchingAddress(Address searchAddress, IEnumerable<Address> addresses)
        {
            return addresses.FirstOrDefault(address =>
                searchAddress.FirstName == address.FirstName &&
                searchAddress.MiddleInitial == address.MiddleInitial &&
                searchAddress.LastName == address.LastName &&
                searchAddress.StreetAddress == address.StreetAddress &&
                searchAddress.OptionalAddress == address.OptionalAddress &&
                searchAddress.City == address.City &&
                searchAddress.State == address.State &&
                searchAddress.ZipCode == address.ZipCode);
        }
    }
}
