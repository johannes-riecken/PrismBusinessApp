

using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using AdventureWorks.WebServices.Models;
using AdventureWorks.WebServices.Repositories;
using AdventureWorks.WebServices.Strings;

namespace AdventureWorks.WebServices.Controllers
{
    public class PaymentMethodController : ApiController
    {
        private readonly IPaymentMethodRepository _paymentMethodRepository;

        public PaymentMethodController():this(new PaymentMethodRepository())
        {
        }

        public PaymentMethodController(IPaymentMethodRepository paymentMethodRepository)
        {
            _paymentMethodRepository = paymentMethodRepository;
        }

        [Authorize]
        public IEnumerable<PaymentMethod> GetAll()
        {
            return _paymentMethodRepository.GetAll(this.User.Identity.Name);
        }

        [Authorize]
        public HttpResponseMessage PostAddress(PaymentMethod paymentMethod)
        {
            if (paymentMethod == null)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, Resources.InvalidAddress);
            }

            if (!ModelState.IsValid)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }

            _paymentMethodRepository.AddUpdate(this.User.Identity.Name, paymentMethod);
            return Request.CreateResponse(HttpStatusCode.OK, true);
        }

        [Authorize]
        public HttpResponseMessage Put(string defaultPaymentMethodId)
        {
            _paymentMethodRepository.SetDefault(this.User.Identity.Name, defaultPaymentMethodId);
            return Request.CreateResponse(HttpStatusCode.OK, true);
        }
    }
}
