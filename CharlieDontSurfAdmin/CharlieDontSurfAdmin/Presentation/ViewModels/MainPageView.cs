using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using CharlieDontSurfAdmin.Models.Orders;
using CharlieDontSurfAdmin.Infrastructure;
using CharlieDontSurfAdmin.Presentation.Events;

namespace CharlieDontSurfAdmin.Presentation.ViewModels
{
    public class MainPageView
    {
        bool running = false;

        Order currentOrder = null; 
        Dictionary<int, Order> currentOrders = new Dictionary<int, Order>();

        public event EventHandler<OrdersRecievedEventArgs> OrdersRecieved;
        public event EventHandler<CurrentOrderChangedEventArgs> CurrentOrderChanged;

        public void Start()
        {
            GetOrders();

            running = true;
            GetNewOrders();

            if (currentOrder != null)
                CurrentOrderChanged(this, new CurrentOrderChangedEventArgs(currentOrder));
        }

        public void Stop()
        {
            running = false;
        }

        public bool HasOrders
        {
            get
            {
                return currentOrders.Count > 0;
            }
        }

        public Order CurrentOrder
        {
            get
            {
                return currentOrder;
            }

            set
            {
                currentOrder = value;
                CurrentOrderChanged(this, new CurrentOrderChangedEventArgs(currentOrder));
            }
        }

        public Dictionary<int, Order> Orders
        {
            get
            {
                return currentOrders;
            }
        }

        private void GetOrders()
        {
            List<Order> orders = Database.GetOrders();

            if (orders.Count > 0)
            {
                currentOrder = orders[0];

                foreach (Order order in orders)
                    currentOrders.Add(order.Id, order);

                OrdersRecieved(this, new OrdersRecievedEventArgs(orders));
            }
        }

        private async void GetNewOrders()
        {
            while (running)
            {
                List<Order> orders = Network.GetOrders();

                if (orders.Count > 0)
                {
                    if (currentOrder == null)
                        currentOrder = orders[0];

                    foreach (Order order in orders)
                    {
                        //currentOrders.Add(order.Id, order);
                        Database.SaveOrder(order);
                    }

                    OrdersRecieved(this, new OrdersRecievedEventArgs(orders));
                }

                await Task.Delay(20000);
            }
        }
    }
}
