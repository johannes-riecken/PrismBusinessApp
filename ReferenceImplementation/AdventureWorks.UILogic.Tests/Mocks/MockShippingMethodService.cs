

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AdventureWorks.UILogic.Models;
using AdventureWorks.UILogic.Services;

namespace AdventureWorks.UILogic.Tests.Mocks
{
    public class MockShippingMethodService : IShippingMethodService
    {
        public Func<Task<IEnumerable<ShippingMethod>>> GetShippingMethodsAsyncDelegate {get;set;}
        public Func<Task<ShippingMethod>> GetBasicShippingMethodAsyncDelegate { get; set; }

        public Task<IEnumerable<ShippingMethod>> GetShippingMethodsAsync()
        {
            return GetShippingMethodsAsyncDelegate();
        }

        public Task<ShippingMethod> GetBasicShippingMethodAsync()
        {
            return GetBasicShippingMethodAsyncDelegate();
        }
    }
}
