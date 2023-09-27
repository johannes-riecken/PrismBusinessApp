

using System;

namespace Microsoft.Practices.Prism.StoreApps
{

    [AttributeUsage(System.AttributeTargets.Property,
                    AllowMultiple = false)]
    public sealed class RestorableStateAttribute : Attribute
    {

    }
}
