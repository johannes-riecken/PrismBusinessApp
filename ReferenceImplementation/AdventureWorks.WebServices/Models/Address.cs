

using System;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using AdventureWorks.WebServices.Repositories;
using AdventureWorks.WebServices.Strings;

namespace AdventureWorks.WebServices.Models
{
    public class Address
    {

        private const string NAMES_REGEX_PATTERN = @"\A\p{L}+([\p{Zs}\-][\p{L}]+)*\z";

        private const string ADDRESS_REGEX_PATTERN = @"\A[\p{L}\p{N}]+([\p{Zs}][\p{L}\p{N}]+)*\z";

        private const string NUMBERS_REGEX_PATTERN = @"\A\p{N}+([\p{N}\-][\p{N}]+)*\z";

        [Required(ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "ErrorRequired")]
        [RegularExpression(NAMES_REGEX_PATTERN, ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "ErrorRegex")]
        public string FirstName { get; set; }

        [RegularExpression(NAMES_REGEX_PATTERN, ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "ErrorRegex")]
        public string MiddleInitial { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "ErrorRequired")]
        [RegularExpression(ADDRESS_REGEX_PATTERN, ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "ErrorRegex")]
        public string LastName { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "ErrorRequired")]
        [RegularExpression(ADDRESS_REGEX_PATTERN, ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "ErrorRegex")]
        public string StreetAddress { get; set; }

        [RegularExpression(ADDRESS_REGEX_PATTERN, ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "ErrorRegex")]
        public string OptionalAddress { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "ErrorRequired")]
        [RegularExpression(ADDRESS_REGEX_PATTERN, ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "ErrorRegex")]
        public string City { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "ErrorRequired")]
        [RegularExpression(ADDRESS_REGEX_PATTERN, ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "ErrorRegex")]
        public string State { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "ErrorRequired")]
        [RegularExpression(NUMBERS_REGEX_PATTERN, ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "ErrorRegex")]
        [CustomValidation(typeof(Address), "ValidateZipCodeState")]
        public string ZipCode { get; set; }

        [Required(ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "ErrorRequired")]
        [RegularExpression(NUMBERS_REGEX_PATTERN, ErrorMessageResourceType = typeof(Resources), ErrorMessageResourceName = "ErrorRegex")]
        public string Phone { get; set; }

        public string Id { get; set; }

        public AddressType AddressType { get; set; }

        public bool IsDefault { get; set; }

        public static ValidationResult ValidateZipCodeState(object value, ValidationContext validationContext)
        {
            bool isValid = false;
            try
            {
                if (value == null)
                {
                    throw new ArgumentNullException("value");
                }

                if (validationContext == null)
                {
                    throw new ArgumentNullException("validationContext");
                }
                
                var address = (Address)validationContext.ObjectInstance;

                if (address.ZipCode.Length < 3)
                {
                    return new ValidationResult(Resources.ErrorZipCodeInvalidLength);
                }

                string stateName = address.State;
                State state = new StateRepository().GetAll().FirstOrDefault(c => c.Name == stateName);
                int zipCode = Convert.ToInt32(address.ZipCode.Substring(0, 3), CultureInfo.InvariantCulture);

                foreach (var range in state.ValidZipCodeRanges)
                {
                    int minValue = Convert.ToInt32(range.Split('-')[0], CultureInfo.InvariantCulture);
                    int maxValue = Convert.ToInt32(range.Split('-')[1], CultureInfo.InvariantCulture);

                    isValid = zipCode >= minValue && zipCode <= maxValue;

                    if (isValid) break;
                }
            }
            catch (ArgumentNullException)
            {
                isValid = false;
            }

            if (isValid)
            {
                return ValidationResult.Success;
            }
            else
            {
                return new ValidationResult(Resources.ErrorInvalidZipCodeInState);
            }
        }
    }
}
