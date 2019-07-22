using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CharlieDontSurf.Models.Shipping
{
    public class ShippingAddress
    {
        int id = 0;
        string recipient;
        string addressLine1;
        string addressLine2 = "";
        string city;
        string county;
        string postcode;
        string country;

        public ShippingAddress()
        {
        }

        public ShippingAddress(int id, string recipient, string addressLine1, string addressLine2, string city, string county, string postcode, string country)
        {
            this.id = id;
            this.recipient = recipient;
            this.addressLine1 = addressLine1;
            this.addressLine2 = addressLine2;
            this.city = city;
            this.county = county;
            this.postcode = postcode;
            this.country = country;
        }

        public ShippingAddress(string recipient, string addressLine1, string addressLine2, string city, string county, string postcode, string country)
        {
            this.recipient = recipient;
            this.addressLine1 = addressLine1;
            this.addressLine2 = addressLine2;
            this.city = city;
            this.county = county;
            this.postcode = postcode;
            this.country = country;
        }

        public int Id
        {
            get
            {
                return id;
            }

            set
            {
                id = value;
            }
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
    }
}