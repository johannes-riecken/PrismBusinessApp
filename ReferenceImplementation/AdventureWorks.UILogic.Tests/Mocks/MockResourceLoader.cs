

using System;
using Microsoft.Practices.Prism.StoreApps.Interfaces;

namespace AdventureWorks.UILogic.Tests.Mocks
{
    public class MockResourceLoader : IResourceLoader
    {
        public MockResourceLoader()
        {
            GetStringDelegate = s => s;
        }

        public Func<string, string> GetStringDelegate { get; set; }
        public string GetString(string resource)
        {
            return GetStringDelegate(resource);
        }
    }
}
