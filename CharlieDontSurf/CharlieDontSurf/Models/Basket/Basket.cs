using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CharlieDontSurf.Models.Basket
{
    public class Basket
    {
        private int id;
        private List<BasketItem> items = new List<BasketItem>();

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

        public List<BasketItem> Items
        {
            get
            {
                return items;
            }
        }

        public void AddItem(BasketItem item)
        {
            items.Add(item);
        }

        public bool IsEmpty
        {
            get
            {
                return items.Count == 0;
            }
        }

        public decimal TotalPrice
        {
            get
            {
                decimal total = 0;

                foreach (BasketItem item in items)
                    total += item.TotalPrice;

                return total;
            }
        }
    }
}