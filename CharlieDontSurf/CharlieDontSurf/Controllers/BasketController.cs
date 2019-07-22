using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

using CharlieDontSurf.Models.Shipping;

namespace CharlieDontSurf.Controllers
{
    public class BasketController : ApiController
    {
        [Authorize]
        [Route("api/Basket/{id}")]
        public string Get(int id)
        {
            return id.ToString();
        }

        public string Post([FromBody] ShippingAddress address)
        {
            return address.Id.ToString();
        }
    }
}
