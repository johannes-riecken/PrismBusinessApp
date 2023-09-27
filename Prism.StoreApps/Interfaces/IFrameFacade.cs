

using System;
using Windows.UI.Xaml;

namespace Microsoft.Practices.Prism.StoreApps.Interfaces
{
    public interface IFrameFacade
    {
        object Content { get; set; }

        void GoBack();

        string GetNavigationState();

        void SetNavigationState(string navigationState);

        bool Navigate(Type sourcePageType, object parameter);

        int BackStackDepth { get; }

        bool CanGoBack { get; }

        event EventHandler<MvvmNavigatedEventArgs> Navigated;

        event EventHandler Navigating;

        object GetValue(DependencyProperty dependencyProperty);

        void SetValue(DependencyProperty dependencyProperty, object value);       
        
        void ClearValue(DependencyProperty dependencyProperty);
    }
}
