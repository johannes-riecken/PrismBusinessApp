

using Microsoft.Practices.Prism.StoreApps.Interfaces;
using Windows.ApplicationModel.Resources;

namespace Microsoft.Practices.Prism.StoreApps
{
    public class ResourceLoaderAdapter : IResourceLoader
    {
        private readonly ResourceLoader _resourceLoader;

        public ResourceLoaderAdapter(ResourceLoader resourceLoader)
        {
            _resourceLoader = resourceLoader;
        }

        public string GetString(string resource)
        {
            return _resourceLoader.GetString(resource);
        }
    }
}
