using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.ComponentModel.DataAnnotations;

namespace CharlieDontSurf.Models.Shipping
{
    public class ShippingAddressViewModel
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Recipient")]
        public string Recipient { get; set; }

        [Required]
        [Display(Name = "Address")]
        public string AddressLine1 { get; set; }

        [Display(Name = "")]
        public string AddressLine2 { get; set; }

        [Required]
        [Display(Name = "City")]
        public string City { get; set; }

        [Required]
        [Display(Name = "County")]
        public string County { get; set; }

        [Required]
        [Display(Name = "Postcode")]
        public string Postcode { get; set; }

        [Required]
        [Display(Name = "Country")]
        public string Country { get; set; }

        List<ShippingAddress> availableShippingAddresses;

        public ShippingAddressViewModel()
        {
        }

        public ShippingAddressViewModel(ShippingAddress address)
        {
            Id = address.Id;
            Recipient = address.Recipient;
            AddressLine1 = address.AddressLine1;
            AddressLine2 = address.AddressLine2;
            City = address.City;
            County = address.County;
            Postcode = address.Postcode;
            Country = address.Country;
        }

        public List<ShippingAddress> AvailableShippingAddresses
        {
            get
            {
                return availableShippingAddresses;
            }

            set
            {
                availableShippingAddresses = value;
            }
        }

        public bool CanAddShippingAddress
        {
            get
            {
                return availableShippingAddresses.Count < 3;
            }
        }

        public bool ShippingAddressesMoreThanOne
        {
            get
            {
                return availableShippingAddresses.Count > 1;
            }
        }
    }
}