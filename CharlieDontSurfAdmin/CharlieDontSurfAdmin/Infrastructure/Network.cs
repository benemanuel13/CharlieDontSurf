using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Net.Http;

using BensJson;
using CharlieDontSurfAdmin.Models.Orders;

namespace CharlieDontSurfAdmin.Infrastructure
{
    public class Network
    {
        public static List<Order> GetOrders()
        {
            HttpClient client = new HttpClient();

            HttpResponseMessage response = client.GetAsync("http://charliedontsurf.net/api/Orders/GetOutstandingOrdersWin").Result;

            string returnedContent = response.Content.ReadAsStringAsync().Result;

            List<Order> orders = new JsonDeserializer<List<Order>>().Deserialize(returnedContent);

            if (orders == null)
                return new List<Order>();

            return orders;
        }
    }
}
