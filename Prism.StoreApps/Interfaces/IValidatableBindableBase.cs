

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace Microsoft.Practices.Prism.StoreApps.Interfaces
{
    public interface IValidatableBindableBase : INotifyPropertyChanged
    {
        bool IsValidationEnabled { get; set; }

        BindableValidator Errors { get; }

        event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;

        ReadOnlyDictionary<string, ReadOnlyCollection<string>> GetAllErrors();

        bool ValidateProperties();

        void SetAllErrors(IDictionary<string, ReadOnlyCollection<string>> entityErrors);
    }
}