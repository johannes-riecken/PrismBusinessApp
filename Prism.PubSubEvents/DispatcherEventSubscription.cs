

using System;
using System.Threading;

namespace Microsoft.Practices.Prism.PubSubEvents
{
    public class DispatcherEventSubscription<TPayload> : EventSubscription<TPayload>
    {
        private readonly SynchronizationContext syncContext;

        public DispatcherEventSubscription(IDelegateReference actionReference, IDelegateReference filterReference, SynchronizationContext context)
            : base(actionReference, filterReference)
        {
            syncContext = context;
        }

        public override void InvokeAction(Action<TPayload> action, TPayload argument)
        {
            syncContext.Post((o) => action((TPayload)o), argument);
        }
    }
}