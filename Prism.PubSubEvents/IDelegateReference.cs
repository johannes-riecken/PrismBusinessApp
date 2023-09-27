

using System;

namespace Microsoft.Practices.Prism.PubSubEvents
{
    public interface IDelegateReference
    {
        Delegate Target { get; }
    }
}