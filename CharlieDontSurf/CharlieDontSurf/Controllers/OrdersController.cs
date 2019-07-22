using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

using CharlieDontSurf.Models.Orders;
using CharlieDontSurf.Infrastructure;

namespace CharlieDontSurf.Controllers
{
    public class OrdersController : ApiController
    {
        [Route("api/Orders/GetOutstandingOrdersWin")]
        public List<Order> GetOutstandingOrdersWin()
        {
            List<Order> outstandingOrders = Database.GetUndownloadedOrders("GetOrdersWin");

            foreach (Order order in outstandingOrders)
                Database.SetOrderDownloaded(order.Id, "SetOrderWin");

            return outstandingOrders;
        }

        [Route("api/Orders/GetOutstandingOrdersMobile")]
        public List<Order> GetOutstandingOrdersMobile()
        {
            List<Order> outstandingOrders = Database.GetUndownloadedOrders("GetOrdersMobile");

            foreach (Order order in outstandingOrders)
                Database.SetOrderDownloaded(order.Id, "SetOrderMobile");

            return outstandingOrders;
        }
    }
}
