using System;
using System.Collections.Generic;
using System.Linq;

namespace CharlieDontSurfAdmin.Models.Orders
{
    public class OrderItem
    {
        public int Id { get; set; }

        public int ItemId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Size { get; set; }
        public string Style { get; set; }
        public int Quantity { get; set; }

        public decimal TotalPrice { get; set; }

        private List<OrderItem> items = new List<OrderItem>();

        public OrderItem()
        { }

        public OrderItem(int id, int itemId, string name, string description, string size, string style, int quantity, decimal totalPrice)
        {
            Id = id;
            ItemId = itemId;
            Name = name;
            Description = description;
            Size = size;
            Style = style;
            Quantity = quantity;
            TotalPrice = totalPrice;
        }

        public OrderItem(int itemId, string name, string description, string size, string style, int quantity, decimal totalPrice)
        {
            ItemId = itemId;
            Name = name;
            Description = description;
            Size = size;
            Style = style;
            Quantity = quantity;
            TotalPrice = totalPrice;
        }

        public void AddOrderItem(OrderItem item)
        {
            items.Add(item);
        }

        public List<OrderItem> Items
        {
            get
            {
                return items;
            }

            set
            {
                items = value;
            }
        }
    }
}