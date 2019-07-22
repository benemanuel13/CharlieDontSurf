using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using CharlieDontSurf.Models.Inventory;
using CharlieDontSurf.Infrastructure;

namespace CharlieDontSurf.Models.Basket
{
    public class BasketItem
    {
        private int id = 0;
        private int itemId = 0;
        private Item item = null;
        int sizeId = 0;
        int styleId = 0;
        private int quantity = 1;
        decimal price;

        bool isSubItem = false;

        private List<BasketItem> subItems = new List<BasketItem>();

        public BasketItem()
        {
        }

        public BasketItem(int itemId, int sizeId, int styleId, int quantity)
        {
            this.itemId = itemId;
            this.item = Database.GetItem(itemId, true);
            this.sizeId = sizeId;
            this.styleId = styleId;
            this.quantity = quantity;
            //this.price = price;
        }

        public BasketItem(int id, int itemId, int sizeId, int styleId, int quantity, decimal price)
        {
            this.id = id;
            this.itemId = itemId;
            this.item = Database.GetItem(itemId, true);
            this.sizeId = sizeId;
            this.styleId = styleId;
            this.quantity = quantity;
            this.price = price;
        }

        public void SetPrice(bool bundlePrice)
        {
            item = Database.GetItem(itemId, true);

            if (!bundlePrice)
                price = item.Price;
            else
                price = item.BundlePrice;
        }

        public void AddSubItem(BasketItem item)
        {
            subItems.Add(item);
        }

        public bool IsSubItem
        {
            get
            {
                return isSubItem;
            }

            set
            {
                isSubItem = value;
            }
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

        public int ItemId
        {
            get
            {
                return itemId;
            }

            set
            {
                itemId = value;
                //this.item = Database.GetItem(value, true);
            }
        }

        public void AddItem(BasketItem item)
        {
            Items.Add(item);
        }

        public List<BasketItem> Items
        {
            get
            {
                return subItems;
            }

            set
            {
                subItems = value;
            }
        }

        public bool HasSubItems
        {
            get
            {
                return subItems.Count > 0;
            }
        }

        public int SizeId
        {
            get
            {
                return sizeId;
            }

            set
            {
                sizeId = value;
            }
        }

        public int StyleId
        {
            get
            {
                return styleId;
            }

            set
            {
                styleId = value;
            }
        }

        public string Name
        {
            get
            {
                return item.Name;
            }
        }

        public string Description
        {
            get
            {
                return item.Description;
            }
        }

        public int Quantity
        {
            get
            {
                return quantity;
            }

            set
            {
                quantity = value;
            }
        }

        public decimal Price
        {
            get
            {
                if (isSubItem)
                    if (quantity > 1)
                        return price * (quantity - 1);
                    else
                        return 0;

                return price;
            }
        }

        public decimal TotalPrice
        {
            get
            {
                if (!isSubItem)
                {
                    if (subItems.Count > 0)
                    {
                        decimal total = price;

                        foreach (BasketItem view in subItems)
                            total += view.Price;

                        return total;
                    }
                    else
                        return price * quantity;
                }
                else
                    return price;
            }
        }

        public Item Item
        {
            get
            {
                return item;
            }
        }
    }
}