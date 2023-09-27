

using System;
using System.Net;
using System.Threading.Tasks;
using AdventureWorks.UILogic.Models;
using Microsoft.Practices.Prism.StoreApps;
using System.Globalization;
using Windows.Web.Http;
using Newtonsoft.Json;
using Windows.Web.Http.Filters;

namespace AdventureWorks.UILogic.Services
{
    public class OrderServiceProxy : IOrderService
    {
        private string _clientBaseUrl = string.Format(CultureInfo.InvariantCulture, "{0}/api/Order/", Constants.ServerAddress);

        public async Task<int> CreateOrderAsync(Order order)
        {
                using (var orderClient = new HttpClient())
                {
                    orderClient.DefaultRequestHeaders.Add("Accept", "application/json");


                    var orderToSend = new Order()
                        {
                            UserId = order.UserId,
                            ShippingMethod = order.ShippingMethod,
                            ShoppingCart = order.ShoppingCart,
                            BillingAddress = order.BillingAddress,
                            ShippingAddress = order.ShippingAddress,
                            PaymentMethod = new PaymentMethod()
                                {
                                    CardNumber = "**** **** **** ****",
                                    CardVerificationCode = "****",
                                    CardholderName = order.PaymentMethod.CardholderName,
                                    ExpirationMonth = order.PaymentMethod.ExpirationMonth,
                                    ExpirationYear = order.PaymentMethod.ExpirationYear,
                                    Phone = order.PaymentMethod.Phone
                                }
                        };

                    string requestUrl = _clientBaseUrl;
                    var stringContent = new HttpStringContent(JsonConvert.SerializeObject(orderToSend), Windows.Storage.Streams.UnicodeEncoding.Utf8, "application/json");
                    var response = await orderClient.PostAsync(new Uri(requestUrl), stringContent);
                    await response.EnsureSuccessWithValidationSupportAsync();

                    var responseContent = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<int>(responseContent);
                }
        }

        public async Task ProcessOrderAsync(Order order)
        {
                using (var orderClient = new HttpClient())
                {
                    orderClient.DefaultRequestHeaders.Add("Accept", "application/json");


                    var orderToSend = new Order()
                    {
                        Id = order.Id,
                        UserId = order.UserId,
                        ShippingMethod = order.ShippingMethod,
                        ShoppingCart = order.ShoppingCart,
                        BillingAddress = order.BillingAddress,
                        ShippingAddress = order.ShippingAddress,
                        PaymentMethod = new PaymentMethod()
                        {
                            CardNumber = "**** **** **** ****",
                            CardVerificationCode = "****",
                            CardholderName = order.PaymentMethod.CardholderName,
                            ExpirationMonth = order.PaymentMethod.ExpirationMonth,
                            ExpirationYear = order.PaymentMethod.ExpirationYear,
                            Phone = order.PaymentMethod.Phone
                        }
                    };

                    string requestUrl = _clientBaseUrl + orderToSend.Id;
                    var stringContent = new HttpStringContent(JsonConvert.SerializeObject(orderToSend), Windows.Storage.Streams.UnicodeEncoding.Utf8, "application/json");
                    var response = await orderClient.PutAsync(new Uri(requestUrl), stringContent);
                    await response.EnsureSuccessWithValidationSupportAsync();
                }
        }
    }
}
