using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using CharlieDontSurf.Models.Shipping;

namespace CharlieDontSurf.Models.Basket
{
    public class BasketView
    {
        private Basket basket;
        private ShippingAddressViewModel shippingAddress;

        public BasketView(Basket basket, ShippingAddressViewModel shippingAddress)
        {
            this.basket = basket;
            this.shippingAddress = shippingAddress;
        }

        public Basket Basket
        {
            get
            {
                return basket;
            }
        }

        public bool HasShippingAddress
        {
            get
            {
                if (shippingAddress == null)
                    return false;

                return true;
            }
        }

        public ShippingAddressViewModel ShippingAddress
        {
            get
            {
                return shippingAddress;
            }
        }
    }
}