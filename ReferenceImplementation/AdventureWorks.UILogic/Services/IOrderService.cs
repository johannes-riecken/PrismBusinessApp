

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using AdventureWorks.UILogic.Models;

namespace AdventureWorks.UILogic.Services
{
    public interface IOrderService
    {
        Task<int> CreateOrderAsync(Order order);

        Task ProcessOrderAsync(Order order);
    }
}
