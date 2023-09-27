

using System;
using Windows.ApplicationModel.Resources;
using Windows.UI.Xaml.Data;

namespace AdventureWorks.Shopper.Converters
{
    public sealed class IsSignInInvalidConverter : IValueConverter
    {
        ResourceLoader resourceLoader = new ResourceLoader();

        public object Convert(object value, Type targetType, object parameter, string language)
        {
            return (value is bool && (bool)value) ? resourceLoader.GetString("ErrorInvalidSignInErrorMessage") : string.Empty;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
