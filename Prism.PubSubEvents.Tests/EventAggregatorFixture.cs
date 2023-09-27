

using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;

namespace Microsoft.Practices.Prism.PubSubEvents.Tests
{
    [TestClass]
    public class EventAggregatorFixture
    {
        [TestMethod]
        public void GetReturnsSingleInstancesOfSameEventType()
        {
            var eventAggregator = new EventAggregator();
            var instance1 = eventAggregator.GetEvent<MockEventBase>();
            var instance2 = eventAggregator.GetEvent<MockEventBase>();

            Assert.AreSame(instance2, instance1);
        }

        public class MockEventBase : EventBase { }
    }
}