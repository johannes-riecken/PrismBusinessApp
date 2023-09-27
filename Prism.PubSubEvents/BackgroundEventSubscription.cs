

using System.Threading;
using System;

namespace Microsoft.Practices.Prism.PubSubEvents
{
    public class BackgroundEventSubscription<TPayload> : EventSubscription<TPayload>
    {
        public BackgroundEventSubscription(IDelegateReference actionReference, IDelegateReference filterReference)
            : base(actionReference, filterReference)
        {
        }

        public override void InvokeAction(Action<TPayload> action, TPayload argument)
        {
            ThreadPool.QueueUserWorkItem( (o) => action(argument) );
        }
    }
}