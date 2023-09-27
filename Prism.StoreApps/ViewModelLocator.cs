

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;
using Windows.UI.Xaml;

namespace Microsoft.Practices.Prism.StoreApps
{
    

    public static class ViewModelLocator
    {
        static Dictionary<string, Func<object>> factories = new Dictionary<string, Func<object>>();
        private static Func<Type, object> defaultViewModelFactory = type => Activator.CreateInstance(type);
        
        private static Func<Type, Type> defaultViewTypeToViewModelTypeResolver = 
            viewType =>
            {
                var viewName = viewType.FullName;
                viewName = viewName.Replace(".Views.", ".ViewModels.");
                var viewAssemblyName = viewType.GetTypeInfo().Assembly.FullName;
                var viewModelName = String.Format(CultureInfo.InvariantCulture, "{0}ViewModel, {1}", viewName, viewAssemblyName);
                return Type.GetType(viewModelName);
            };

        public static void SetDefaultViewModelFactory(Func<Type, object> viewModelFactory)
        {
            defaultViewModelFactory = viewModelFactory;
        }

        public static void SetDefaultViewTypeToViewModelTypeResolver(Func<Type, Type> viewTypeToViewModelTypeResolver)
        {
            defaultViewTypeToViewModelTypeResolver = viewTypeToViewModelTypeResolver;
        }

        #region Attached property with convention-or-mapping based approach

        public static DependencyProperty AutoWireViewModelProperty =
            DependencyProperty.RegisterAttached("AutoWireViewModel", typeof(bool), typeof(ViewModelLocator), 
            new PropertyMetadata(false, AutoWireViewModelChanged));

        public static bool GetAutoWireViewModel(DependencyObject obj)
        {
            if (obj != null)
            {
                return (bool) obj.GetValue(AutoWireViewModelProperty);
            }
            return false;
        }

        public static void SetAutoWireViewModel(DependencyObject obj, bool value)
        {
            if (obj != null)
            {
                obj.SetValue(AutoWireViewModelProperty, value);
            }
        }

        #endregion

        private static void AutoWireViewModelChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            FrameworkElement view = d as FrameworkElement;
            if (view == null) return; // Incorrect hookup, do no harm

            object viewModel = GetViewModelForView(view);
            if (viewModel == null)
            {
                var viewModelType = defaultViewTypeToViewModelTypeResolver(view.GetType());
                if (viewModelType == null) return;

                viewModel = defaultViewModelFactory(viewModelType);
            }
            view.DataContext = viewModel;
        }

        private static object GetViewModelForView(FrameworkElement view)
        {
            if (factories.ContainsKey(view.GetType().ToString()))
                return factories[view.GetType().ToString()]();
            return null;
        }

        public static void Register(string viewTypeName, Func<object> factory)
        {
            factories[viewTypeName] = factory;
        }
    }
}
