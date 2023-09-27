

using System.Collections.Generic;
using System.Threading.Tasks;
using AdventureWorks.UILogic.Models;

namespace AdventureWorks.UILogic.Services
{
    public interface IAddressService
    {
        Task<IReadOnlyCollection<Address>> GetAddressesAsync();
        Task SaveAddressAsync(Address address);
        Task SetDefault(string defaultAddressId, AddressType addressType);
    }
}
