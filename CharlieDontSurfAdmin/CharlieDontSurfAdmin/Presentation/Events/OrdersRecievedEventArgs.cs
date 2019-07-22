using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using CharlieDontSurfAdmin.Models.Orders;

namespace CharlieDontSurfAdmin.Presentation.Events
{
    public class OrdersRecievedEventArgs : EventArgs
    {
        public List<Order> Orders { get; set; }

        public OrdersRecievedEventArgs(List<Order> orders)
        {
            Orders = orders;
        }
    }
}
