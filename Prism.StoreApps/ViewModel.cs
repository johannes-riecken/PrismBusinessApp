

using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.Practices.Prism.StoreApps.Interfaces;
using Windows.UI.Xaml.Navigation;

namespace Microsoft.Practices.Prism.StoreApps
{
    public class ViewModel : BindableBase, INavigationAware
    {
        public virtual void OnNavigatedTo(object navigationParameter, NavigationMode navigationMode, Dictionary<string, object> viewModelState)
        {
            if (viewModelState != null)
            {
                RestoreViewModel(viewModelState, this);
            }
        }

        public virtual void OnNavigatedFrom(Dictionary<string, object> viewModelState, bool suspending)
        {
            if (viewModelState != null)
            {
                FillStateDictionary(viewModelState, this);
            }
        }

        static public T RetrieveEntityStateValue<T>(string entityStateKey, IDictionary<string, object> viewModelState)
        {
            if (viewModelState != null && viewModelState.ContainsKey(entityStateKey))
            {
                return (T)viewModelState[entityStateKey];
            }

            return default(T);
        }

        public static void AddEntityStateValue(string viewModelStateKey, object viewModelStateValue, IDictionary<string, object> viewModelState)
        {
            if (viewModelState != null)
            {
                viewModelState[viewModelStateKey] = viewModelStateValue;
            }
        }

        private static void FillStateDictionary(IDictionary<string, object> viewModelState, object viewModel)
        {
            var viewModelProperties = viewModel.GetType().GetRuntimeProperties().Where(
                                                            c => c.GetCustomAttribute(typeof(RestorableStateAttribute)) != null);

            foreach (PropertyInfo propertyInfo in viewModelProperties)
            {
                viewModelState[propertyInfo.Name] = propertyInfo.GetValue(viewModel);
            }
        }

        private static void RestoreViewModel(IDictionary<string, object> viewModelState, object viewModel)
        {
            var viewModelProperties = viewModel.GetType().GetRuntimeProperties().Where(
                                                            c => c.GetCustomAttribute(typeof(RestorableStateAttribute)) != null);

            foreach (PropertyInfo propertyInfo in viewModelProperties)
            {
                if (viewModelState.ContainsKey(propertyInfo.Name))
                {
                    propertyInfo.SetValue(viewModel, viewModelState[propertyInfo.Name]);
                }
            }
        }
    }
}
