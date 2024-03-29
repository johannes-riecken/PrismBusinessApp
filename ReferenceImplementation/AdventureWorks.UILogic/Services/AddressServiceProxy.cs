

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net;
using System.Threading.Tasks;
using AdventureWorks.UILogic.Models;
using Microsoft.Practices.Prism.StoreApps;
using System.Globalization;
using Newtonsoft.Json;
using Windows.Web.Http;
using Windows.Web.Http.Filters;

namespace AdventureWorks.UILogic.Services
{
    public class AddressServiceProxy : IAddressService
    {
        private readonly IAccountService _accountService;
        private string _clientBaseUrl = string.Format(CultureInfo.InvariantCulture, "{0}/api/Address", Constants.ServerAddress);

        public AddressServiceProxy(IAccountService accountService)
        {
            _accountService = accountService;
        }

        public async Task<IReadOnlyCollection<Address>> GetAddressesAsync()
        {
            using (var client = new HttpClient())
            {
                var response = await client.GetAsync(new Uri(_clientBaseUrl));
                response.EnsureSuccessStatusCode();
                var responseContent = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<ReadOnlyCollection<Address>>(responseContent);
            }
        }

        public async Task SaveAddressAsync(Address address)
        {
            using (var client = new HttpClient())
            {
                var serializedAddress = JsonConvert.SerializeObject(address);

                var response = await client.PostAsync(new Uri(_clientBaseUrl), new HttpStringContent(serializedAddress, Windows.Storage.Streams.UnicodeEncoding.Utf8, "application/json"));
                await response.EnsureSuccessWithValidationSupportAsync();
            }
        }

        public async Task SetDefault(string defaultAddressId, AddressType addressType)
        {
            using (var client = new HttpClient())
            {
                var response = await client.PutAsync(new Uri(_clientBaseUrl + "/?defaultAddressId=" + defaultAddressId + "&addressType=" + addressType), null);
                await response.EnsureSuccessWithValidationSupportAsync();
            }
        }
    }
}
