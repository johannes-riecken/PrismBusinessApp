

using AdventureWorks.UILogic.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AdventureWorks.UILogic.Services
{
    public interface IPaymentMethodService
    {
        Task<IReadOnlyCollection<PaymentMethod>> GetPaymentMethodsAsync();
        Task SavePaymentMethodAsync(PaymentMethod paymentMethod);
        Task SetDefault(string defaultPaymentMethodId);
    }
}
