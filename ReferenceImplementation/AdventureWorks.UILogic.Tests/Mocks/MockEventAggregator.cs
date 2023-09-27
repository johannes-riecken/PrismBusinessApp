

using System;
using Microsoft.Practices.Prism.PubSubEvents;

namespace AdventureWorks.UILogic.Tests.Mocks
{
    public class MockEventAggregator : IEventAggregator
    {
        public Func<Type, EventBase> GetEventDelegate { get; set; }

        public TEventType GetEvent<TEventType>() where TEventType : EventBase, new()
        {
            return (TEventType)GetEventDelegate(typeof(TEventType));
        }
    }
}
