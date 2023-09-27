

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using AdventureWorks.WebServices.Models;
using AdventureWorks.WebServices.Repositories;

namespace AdventureWorks.WebServices.Controllers
{
    public class ShippingMethodController : ApiController
    {
        private IRepository<ShippingMethod> _shippingMethodRepository;

        public ShippingMethodController()
            : this(new ShippingMethodRepository())
        { }

        public ShippingMethodController(IRepository<ShippingMethod> shippingMethodRepository)
        {
            _shippingMethodRepository = shippingMethodRepository;
        }

        [HttpGet]
        [ActionName("defaultAction")]
        public IEnumerable<ShippingMethod> GetShippingMethods()
        {
            return _shippingMethodRepository.GetAll();
        }

        [HttpGet]
        [ActionName("basic")]
        public ShippingMethod GetBasicShippingMethod()
        {
            return _shippingMethodRepository.GetAll().FirstOrDefault(c => c.Description.Contains("Standard"));
        }
    }
}