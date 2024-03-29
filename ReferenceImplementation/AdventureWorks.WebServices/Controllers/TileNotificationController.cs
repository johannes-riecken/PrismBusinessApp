

using System.Globalization;
using AdventureWorks.WebServices.Repositories;
using System;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Xml;

namespace AdventureWorks.WebServices.Controllers
{
    public class TileNotificationController : ApiController
    {
        private IProductRepository _productRepository;

        public TileNotificationController()
            : this(new ProductRepository())
        { }

        public TileNotificationController(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }


        public HttpResponseMessage GetTileNotification()
        {
            var tileXml = GetDefaultTileXml("http://localhost:2112/Images/hotrodbike_red_large.jpg",
                                            "Mountain-400-W Red, 42");
            tileXml = string.Format(CultureInfo.InvariantCulture, tileXml, DateTime.Now.ToShortDateString(), DateTime.Now.ToShortTimeString());

           var response = new HttpResponseMessage();

            response.StatusCode = System.Net.HttpStatusCode.OK;
            response.Content = new StringContent(tileXml);

            response.Content.Headers.ContentType = 
                new System.Net.Http.Headers.MediaTypeHeaderValue("text/xml");
            return response;
        } 



        public HttpResponseMessage GetSecondaryTileNotification(string id)
        {
            var product = _productRepository.GetProduct(id);
            if (product == null)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }

            var tileXml = GetSecondaryTileXml(product.ImageUri.AbsoluteUri, product.Title);

            var response = new HttpResponseMessage();

            response.StatusCode = System.Net.HttpStatusCode.OK;
            response.Content = new StringContent(tileXml);

            response.Content.Headers.ContentType =
                new System.Net.Http.Headers.MediaTypeHeaderValue("text/xml");
            return response;
        }

        private static string GetDefaultTileXml(string imageUri, string title)
        {
            var tileXml = @"<tile>
                                <visual>
                                <binding template=""TileWidePeekImage01"">
                                    <image id=""1"" src=""{0}"" alt=""alt text""/>
                                    <text id=""1"">Today's Deals</text>
                                    <text id=""2"">{1}. Updated: {2} {3}</text>
                                </binding>
                                <binding template=""TileSquarePeekImageAndText02"">
                                    <image id=""1"" src=""{0}"" alt=""alt text""/>
                                    <text id=""1"">Today's Deals</text>
                                    <text id=""2"">{1}. Updated: {2} {3}</text>
                                </binding> 
                                </visual>
                            </tile>";

            return string.Format(CultureInfo.InvariantCulture, tileXml, imageUri, title, 
                DateTime.Now.ToShortDateString(), DateTime.Now.ToShortTimeString());
        }

        private static string GetSecondaryTileXml(string imageUri, string title)
        {
            var tileXml = @"<tile>
                                <visual>
                                <binding template=""TileWidePeekImage03"">
                                    <image id=""1"" src=""{0}"" alt=""alt text""/>
                                    <text id=""1"">{1}</text>
                                </binding>
                                <binding template=""TileSquarePeekImageAndText04"">
                                    <image id=""1"" src=""{0}"" alt=""alt text""/>
                                    <text id=""1"">{1}</text>
                                </binding> 
                                </visual>
                            </tile>";

            return string.Format(CultureInfo.InvariantCulture, tileXml, imageUri, title);
        }
    }
}
