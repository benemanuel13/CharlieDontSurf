using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using CharlieDontSurf.Infrastructure;

namespace CharlieDontSurf.Models.Inventory
{
    public class Item
    {
        int id;
        string name;
        string description;
        string fullDescription;
        decimal price;
        decimal bundlePrice;

        List<ItemSize> sizes = new List<ItemSize>();
        List<ItemStyle> styles = new List<ItemStyle>();

        List<Item> subItems = new List<Item>();

        public Item(int id, string name, string description, string fullDescription, decimal price, decimal bundlePrice)
        {
            this.id = id;
            this.name = name;
            this.description = description;
            this.fullDescription = fullDescription;
            this.price = price;
            this.bundlePrice = bundlePrice;
        }

        public void AddSize(ItemSize size)
        {
            sizes.Add(size);
        }

        public List<ItemSize> Sizes
        {
            get
            {
                return sizes;
            }
        }

        public List<ItemStyle> Styles
        {
            get
            {
                return styles;
            }
        }

        public bool HasSizes
        {
            get
            {
                return sizes.Count() > 0;
            }
        }

        public bool HasStyles
        {
            get
            {
                return styles.Count() > 0;
            }
        }
        public bool HasSubItems
        {
            get
            {
                return subItems.Count() > 0;
            }
        }

        public void AddSubItem(Item item)
        {
            subItems.Add(item);
        }

        public List<Item> SubItems
        {
            get
            {
                return subItems;
            }
        }

        public int Id
        {
            get
            {
                return id;
            }
        }

        public string Name
        {
            get
            {
                return name;
            }
        }

        public string Description
        {
            get
            {
                return description;
            }
        }

        public string FullDescription
        {
            get
            {
                return fullDescription;
            }
        }

        public decimal Price
        {
            get
            {
                return price;
            }
        }

        public decimal BundlePrice
        {
            get
            {
                return bundlePrice;
            }
        }
    }
}