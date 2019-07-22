using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using CharlieDontSurf.Models.Inventory;

namespace CharlieDontSurf.Models.Shopping
{
    public class ShoppingModel
    {
        List<ItemType> itemTypes;
        List<Item> items;

        public List<ItemType> ItemTypes
        {
            get
            {
                return itemTypes;
            }

            set
            {
                itemTypes = value;
            }
        }

        public List<Item> Items
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