

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Reflection;
using Windows.ApplicationModel.Resources;

namespace Microsoft.Practices.Prism.StoreApps
{
    public class BindableValidator : INotifyPropertyChanged
    {
        private readonly INotifyPropertyChanged _entityToValidate;

        private IDictionary<string, ReadOnlyCollection<string>> _errors = new Dictionary<string, ReadOnlyCollection<string>>();

        public static readonly ReadOnlyCollection<string> EmptyErrorsCollection = new ReadOnlyCollection<string>(new List<string>());

        private Func<string, string, string> _getResourceDelegate;

        public BindableValidator(INotifyPropertyChanged entityToValidate, Func<string, string, string> getResourceDelegate)
            : this(entityToValidate)
        {
            _getResourceDelegate = getResourceDelegate;
        }

        public BindableValidator(INotifyPropertyChanged entityToValidate)
        {
            if (entityToValidate == null)
            {
                throw new ArgumentNullException("entityToValidate");
            }

            _entityToValidate = entityToValidate;
            IsValidationEnabled = true;
            _getResourceDelegate = (mapId, key) =>
            {
                var resourceLoader = ResourceLoader.GetForCurrentView(mapId);
                return resourceLoader.GetString(key);
            };
        }

        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;

        public event PropertyChangedEventHandler PropertyChanged;

        public ReadOnlyCollection<string> this[string propertyName]
        {
            get
            {
                return _errors.ContainsKey(propertyName) ? _errors[propertyName] : EmptyErrorsCollection;
            }
        }

        public IDictionary<string, ReadOnlyCollection<string>> Errors
        {
            get { return _errors; }
        }

        public bool IsValidationEnabled { get; set; }

        public ReadOnlyDictionary<string, ReadOnlyCollection<string>> GetAllErrors()
        {
            return new ReadOnlyDictionary<string, ReadOnlyCollection<string>>(_errors);
        }

        public void SetAllErrors(IDictionary<string, ReadOnlyCollection<string>> entityErrors)
        {
            if (entityErrors == null)
            {
                throw new ArgumentNullException("entityErrors");
            }

            _errors.Clear();

            foreach (var item in entityErrors)
            {
                SetPropertyErrors(item.Key, item.Value);
            }

            OnPropertyChanged("Item[]");
            OnErrorsChanged(string.Empty);
        }

        public bool ValidateProperty(string propertyName)
        {
            if (string.IsNullOrEmpty(propertyName))
            {
                throw new ArgumentNullException("propertyName");
            }

            var propertyInfo = _entityToValidate.GetType().GetRuntimeProperty(propertyName);

            if (propertyInfo == null)
            {
                var errorString = _getResourceDelegate(Constants.StoreAppsInfrastructureResourceMapId, "InvalidPropertyNameException");

                throw new ArgumentException(errorString, propertyName);
            }

            var propertyErrors = new List<string>();
            bool isValid = TryValidateProperty(propertyInfo, propertyErrors);
            bool errorsChanged = SetPropertyErrors(propertyInfo.Name, propertyErrors);

            if (errorsChanged)
            {
                OnErrorsChanged(propertyName);
                OnPropertyChanged(string.Format(CultureInfo.CurrentCulture, "Item[{0}]", propertyName));
            }

            return isValid;
        }

        public bool ValidateProperties()
        {
            var propertiesWithChangedErrors = new List<string>();

            var propertiesToValidate = _entityToValidate.GetType()
                                                        .GetRuntimeProperties()
                                                        .Where(c => c.GetCustomAttributes(typeof(ValidationAttribute)).Any());

            foreach (PropertyInfo propertyInfo in propertiesToValidate)
            {
                var propertyErrors = new List<string>();
                TryValidateProperty(propertyInfo, propertyErrors);

                bool errorsChanged = SetPropertyErrors(propertyInfo.Name, propertyErrors);
                if (errorsChanged && !propertiesWithChangedErrors.Contains(propertyInfo.Name))
                {
                    propertiesWithChangedErrors.Add(propertyInfo.Name);
                }
            }

            foreach (string propertyName in propertiesWithChangedErrors)
            {
                OnErrorsChanged(propertyName);
                OnPropertyChanged(string.Format(CultureInfo.CurrentCulture, "Item[{0}]", propertyName));
            }

            return _errors.Values.Count == 0;
        }

        private bool TryValidateProperty(PropertyInfo propertyInfo, List<string> propertyErrors)
        {
            var results = new List<ValidationResult>();
            var context = new ValidationContext(_entityToValidate) { MemberName = propertyInfo.Name };
            var propertyValue = propertyInfo.GetValue(_entityToValidate);

            bool isValid = Validator.TryValidateProperty(propertyValue, context, results);

            if (results.Any())
            {
                propertyErrors.AddRange(results.Select(c => c.ErrorMessage));
            }

            return isValid;
        }

        private bool SetPropertyErrors(string propertyName, IList<string> propertyNewErrors)
        {
            bool errorsChanged = false;

            if (!_errors.ContainsKey(propertyName))
            {
                if (propertyNewErrors.Count > 0)
                {
                    _errors.Add(propertyName, new ReadOnlyCollection<string>(propertyNewErrors));
                    errorsChanged = true;
                }
            }
            else
            {
                if (propertyNewErrors.Count != _errors[propertyName].Count || _errors[propertyName].Intersect(propertyNewErrors).Count() != propertyNewErrors.Count())
                {
                    if (propertyNewErrors.Count > 0)
                    {
                        _errors[propertyName] = new ReadOnlyCollection<string>(propertyNewErrors);
                    }
                    else
                    {
                        _errors.Remove(propertyName);
                    }

                    errorsChanged = true;
                }
            }

            return errorsChanged;
        }

        private void OnPropertyChanged(string propertyName)
        {
            var eventHandler = PropertyChanged;
            if (eventHandler != null)
            {
                eventHandler(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        private void OnErrorsChanged(string propertyName)
        {
            var eventHandler = ErrorsChanged;
            if (eventHandler != null)
            {
                eventHandler(this, new DataErrorsChangedEventArgs(propertyName));
            }
        }
    }
}
