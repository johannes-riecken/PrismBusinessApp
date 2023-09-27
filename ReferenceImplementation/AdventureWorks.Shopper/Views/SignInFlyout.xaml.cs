

using Microsoft.Practices.Prism.StoreApps;
using Microsoft.Practices.Prism.StoreApps.Interfaces;
using Windows.ApplicationModel.Search;
using Windows.System;
using Windows.UI.Xaml.Controls;

namespace AdventureWorks.Shopper.Views
{
    public sealed partial class SignInFlyout : SettingsFlyout
    {
        public SignInFlyout()
        {
            this.InitializeComponent();
            this.PasswordBox.KeyDown += PasswordBox_KeyDown;

            var searchPane = SearchPane.GetForCurrentView();
            searchPane.ShowOnKeyboardInput = false;

            var viewModel = this.DataContext as IFlyoutViewModel;
            viewModel.CloseFlyout = () => this.Hide();
        }

        void PasswordBox_KeyDown(object sender, Windows.UI.Xaml.Input.KeyRoutedEventArgs e)
        {
            if (e.Key == VirtualKey.Enter)
            {
                SubmitButton.Command.Execute(null);
            }
        }
    }
}
