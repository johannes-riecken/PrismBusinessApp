

using System.Threading.Tasks;
using AdventureWorks.UILogic.Models;

namespace AdventureWorks.UILogic.Repositories
{
    public interface IOrderRepository
    {
        Task CreateBasicOrderAsync(string userId, ShoppingCart shoppingCart, Address shippingAddress,
                                          Address billingAddress, PaymentMethod paymentMethod);

        Task<Order> CreateOrderAsync(string userId, ShoppingCart shoppingCart, Address shippingAddress,
                                     Address billingAddress, PaymentMethod paymentMethod, ShippingMethod shippingMethod);

        Order CurrentOrder { get; }
    }
}
