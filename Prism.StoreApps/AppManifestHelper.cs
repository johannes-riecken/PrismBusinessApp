

using System.Xml.Linq;

namespace Microsoft.Practices.Prism.StoreApps
{
    public static class AppManifestHelper
    {
        private static readonly XDocument manifest = XDocument.Load("AppxManifest.xml", LoadOptions.None);
        private static readonly XNamespace xNamespace = XNamespace.Get("http://schemas.microsoft.com/appx/2010/manifest");

        public static bool IsSearchDeclared()
        {
            var extensions = manifest.Descendants(xNamespace + "Extension");
            foreach (var extension in extensions)
            {
                if (extension.Attribute("Category") != null && extension.Attribute("Category").Value == "windows.search")
                {
                    return true;
                }
            }

            return false;
        }

        public static string GetApplicationId()
        {
            var applications = manifest.Descendants(xNamespace + "Application");
            foreach (var application in applications)
            {
                if (application.Attribute("Id") != null)
                {
                    return application.Attribute("Id").Value;
                }
            }
            return string.Empty;
        }
    }
}
