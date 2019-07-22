using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using CharlieDontSurfAdmin.Models.Orders;

namespace CharlieDontSurfAdmin.Presentation.Events
{
    public class CurrentOrderChangedEventArgs : EventArgs
    {
        public Order CurrentOrder { get; set; }

        public CurrentOrderChangedEventArgs(Order newOrder)
        {
            CurrentOrder = newOrder;
        }
    }
}
