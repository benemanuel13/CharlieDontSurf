using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.Web.Mvc;

using CharlieDontSurf.Models.Basket;

namespace CharlieDontSurf.Models.Inventory
{
    public class ItemView
    {
        public int Id { get; set; }
        public Item Item { get; set; }
        public bool HasParent { get; set; }
        public int SizeId { get; set; }
        public List<ItemSize> ItemSizes { get; set; }
        public List<SelectListItem> SelectableItemSizes = new List<SelectListItem>();

        public int StyleId { get; set; }
        public List<ItemStyle> ItemStyles { get; set; }
        public List<SelectListItem> SelectableItemStyles = new List<SelectListItem>();

        public List<ItemView> subItems = new List<ItemView>();
        public List<SelectListItem> SelectableSubItems = new List<SelectListItem>();

        public bool BasketBased { get; set; }
        public int Quantity = 1;
        public List<SelectListItem> SelectableQuantities = new List<SelectListItem>();

        public ItemView(Item item, bool hasParent)
        {
            HasParent = true;
            Init(item);
        }

        public ItemView(Item item)
        {
            HasParent = false;
            Init(item);
        }

        private void Init(Item item)
        {
            Id = 0;
            BasketBased = false;

            if (!item.HasSubItems)
            {
                Item = item;
                if (!item.HasSizes)
                    SizeId = 0;
                else
                {
                    ItemSizes = item.Sizes;
                    SizeId = item.Sizes[0].Id;                

                    if(!HasParent)
                        SetSizeSelections();
                }

                ItemSizes = item.Sizes;
                SetQuantitySelections();
            }
            else
            {
                Item = item;

                foreach (Item subItem in item.SubItems)
                {
                    ItemView view = new ItemView(subItem, true);
                    subItems.Add(view);

                    SelectListItem selItem = new SelectListItem();
                    selItem.Text = subItem.Name;
                    selItem.Value = subItem.Id.ToString();

                    SelectableSubItems.Add(selItem);
                }

                SetQuantitySelections();
            }
        }

        public List<ItemView> SubItems
        {
            get
            {
                return subItems;
            }
        }

        public ItemView(BasketItem item, bool hasParent)
        {
            HasParent = true;
            InitBasket(item);
        }

        public ItemView(BasketItem item)
        {
            HasParent = false;
            InitBasket(item);
        }

        private void InitBasket(BasketItem item)
        {
            Id = item.Id;

            BasketBased = true;

            if (!item.HasSubItems)
            {
                Item = item.Item;
                if (!item.Item.HasSizes)
                    SizeId = 0;
                else
                {
                    ItemSizes = item.Item.Sizes;
                    SizeId = item.SizeId;

                    if (!HasParent)
                        SetSizeSelections();
                }

                ItemSizes = item.Item.Sizes;
                Quantity = item.Quantity;
                SetQuantitySelections();
            }
            else
            {
                Item = item.Item;

                foreach (BasketItem subItem in item.Items)
                {
                    ItemView view = new ItemView(subItem, true);
                    subItems.Add(view);

                    SelectListItem selItem = new SelectListItem();
                    selItem.Text = subItem.Item.Name;
                    selItem.Value = subItem.Item.Id.ToString();

                    SelectableSubItems.Add(selItem);
                }
            }
        }

        private void SetQuantitySelections()
        {
            if (Item.HasSubItems)
            {
                for (int i = 1; i < 4; i++)
                {
                    SelectListItem item = new SelectListItem();
                    item.Text = i.ToString();
                    item.Value = i.ToString();
                    if (i == subItems[0].Quantity)
                        item.Selected = true;

                    SelectableQuantities.Add(item);
                }
            }
            else
            {
                for (int i = 1; i < 4; i++)
                {
                    SelectListItem item = new SelectListItem();
                    item.Text = i.ToString();
                    item.Value = i.ToString();
                    if (i == Quantity)
                        item.Selected = true;

                    SelectableQuantities.Add(item);
                }
            }
        }

        private void SetSizeSelections()
        {
            if (Item.HasSubItems)
            {
                foreach (ItemSize size in subItems[0].ItemSizes)
                {
                    SelectListItem item = new SelectListItem();
                    item.Text = size.Description;
                    item.Value = size.Id.ToString();
                    if (size.Id == subItems[0].SizeId)
                        item.Selected = true;

                    SelectableItemSizes.Add(item);
                }
            }
            else
            {
                foreach (ItemSize size in ItemSizes)
                {
                    SelectListItem item = new SelectListItem();
                    item.Text = size.Description;
                    item.Value = size.Id.ToString();
                    if (size.Id == SizeId)
                        item.Selected = true;

                    SelectableItemSizes.Add(item);
                }
            }
        }

        private void SetStyleSelections()
        {
            if (Item.HasSubItems)
            {
                foreach (ItemStyle size in subItems[0].ItemStyles)
                {
                    SelectListItem item = new SelectListItem();
                    item.Text = size.Description;
                    item.Value = size.Id.ToString();
                    if (size.Id == subItems[0].StyleId)
                        item.Selected = true;

                    SelectableItemStyles.Add(item);
                }
            }
            else
            {
                foreach (ItemStyle size in ItemStyles)
                {
                    SelectListItem item = new SelectListItem();
                    item.Text = size.Description;
                    item.Value = size.Id.ToString();
                    if (size.Id == StyleId)
                        item.Selected = true;

                    SelectableItemStyles.Add(item);
                }
            }
        }

        public bool HasSizes
        {
            get
            {
                return Item.HasSizes;
            }
        }

        public bool HasStyles
        {
            get
            {
                return Item.HasStyles;
            }
        }
    }
}