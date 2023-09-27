

using System.Collections.Generic;
using System.Globalization;
using AdventureWorks.WebServices.Repositories;
using System;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Xml;
using AdventureWorks.WebServices.Models;
using AdventureWorks.WebServices.Strings;

namespace AdventureWorks.WebServices.Controllers
{
    public class AddressController : ApiController
    {
        private readonly IAddressRepository _addressRepository;

        public AddressController():this(new AddressRepository())
        {
            
        }

        public AddressController(IAddressRepository addressRepository)
        {
            _addressRepository = addressRepository;
        }

        [Authorize]
        public IEnumerable<Address> GetAll()
        {
            return _addressRepository.GetAll(this.User.Identity.Name);
        }
        
        [Authorize]
        public HttpResponseMessage PostAddress(Address address)
        {
            if (address == null)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, Resources.InvalidAddress);
            }

            if (!ModelState.IsValid)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }

            _addressRepository.AddUpdate(this.User.Identity.Name, address);
            return Request.CreateResponse(HttpStatusCode.OK, true);
        }

        [Authorize]
        public HttpResponseMessage Put(string defaultAddressId, AddressType addressType)
        {
            _addressRepository.SetDefault(this.User.Identity.Name, defaultAddressId, addressType);
            return Request.CreateResponse(HttpStatusCode.OK, true);
        }
    }
}
