

using Windows.ApplicationModel.Resources;

namespace ValidationQuickStart.Models
{
    public static class ErrorMessagesHelper
    {
        private static readonly ResourceLoader _resourceLoader = new ResourceLoader();

        public static string FirstNameRequired
        {
            get { return _resourceLoader.GetString("ErrorFirstNameRequired"); }
        }

        public static string FirstNameRegex
        {
            get { return _resourceLoader.GetString("ErrorFirstNameRegex"); }
        }

        public static string MiddleNameRequired
        {
            get { return _resourceLoader.GetString("ErrorMiddleNameRequired"); }
        }

        public static string MiddleNameFirstName
        {
            get { return _resourceLoader.GetString("ErrorMiddleNameFirstName"); }
        }

        public static string MiddleNameRegex
        {
            get { return _resourceLoader.GetString("ErrorMiddleNameRegex"); }
        }

        public static string LastNameRequired
        {
            get { return _resourceLoader.GetString("ErrorLastNameRequired"); }
        }

        public static string LastNameRegex
        {
            get { return _resourceLoader.GetString("ErrorLastNameRegex"); }
        }
    }
}
