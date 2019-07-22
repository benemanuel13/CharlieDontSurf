using System;
using System.Collections.Generic;
using System.Linq;

namespace CharlieDontSurfAdmin.Models.Orders
{
    public class Order
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        private List<OrderItem> items = new List<OrderItem>();

        string recipient;
        string addressLine1;
        string addressLine2 = "";
        string city;
        string county;
        string postcode;
        string country;

        public Order()
        {
        }

        public Order(int id, string userId, string recipient, string addressLine1, string addressLine2, string city, string county, string postcode, string country)
        {
            Id = id;
            UserId = userId;
            this.recipient = recipient;
            this.addressLine1 = addressLine1;
            this.addressLine2 = addressLine2;
            this.city = city;
            this.county = county;
            this.postcode = postcode;
            this.country = country;
        }

        public Order(string userId, string recipient, string addressLine1, string addressLine2, string city, string county, string postcode, string country)
        {
            UserId = userId;
            this.recipient = recipient;
            this.addressLine1 = addressLine1;
            this.addressLine2 = addressLine2;
            this.city = city;
            this.county = county;
            this.postcode = postcode;
            this.country = country;
        }

        public void AddOrderItem(OrderItem item)
        {
            items.Add(item);
        }

        public string Recipient
        {
            get
            {
                return recipient;
            }

            set
            {
                recipient = value;
            }
        }

        public string AddressLine1
        {
            get
            {
                return addressLine1;
            }

            set
            {
                addressLine1 = value;
            }
        }

        public string AddressLine2
        {
            get
            {
                return addressLine2;
            }

            set
            {
                addressLine2 = value;
            }
        }

        public string City
        {
            get
            {
                return city;
            }

            set
            {
                city = value;
            }
        }

        public string County
        {
            get
            {
                return county;
            }

            set
            {
                county = value;
            }
        }

        public string Postcode
        {
            get
            {
                return postcode;
            }

            set
            {
                postcode = value;
            }
        }

        public string Country
        {
            get
            {
                return country;
            }

            set
            {
                country = value;
            }
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