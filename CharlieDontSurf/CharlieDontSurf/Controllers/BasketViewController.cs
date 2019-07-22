using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using Microsoft.AspNet.Identity;

using CharlieDontSurf.Models.Basket;
using CharlieDontSurf.Models.Shipping;
using CharlieDontSurf.Models.Inventory;
using CharlieDontSurf.Infrastructure;

using BensJson;

namespace CharlieDontSurf.Controllers
{
    public class BasketViewController : Controller
    {
        // GET: BasketView
        [Authorize]
        public ActionResult Index()
        {
            Basket currentBasket = Database.GetCurrentBasket(User.Identity.GetUserId(), true);
            ShippingAddress shippingAddress = Database.GetCurrentShippingAddress(User.Identity.GetUserId());
            ShippingAddressViewModel viewModel = null;

            if (shippingAddress != null)
            {
                viewModel = new ShippingAddressViewModel(shippingAddress);
                viewModel.AvailableShippingAddresses = Database.GetShippingAddresses(User.Identity.GetUserId());
            }
            
            BasketView view = new BasketView(currentBasket, viewModel);

            return View(view);
        }

        [Authorize]
        public ActionResult ShowCurrentShippingAddress()
        {
            ShippingAddress shippingAddress = Database.GetCurrentShippingAddress(User.Identity.GetUserId());
            ShippingAddressViewModel viewModel = null;

            if (shippingAddress != null)
            {
                viewModel = new ShippingAddressViewModel(shippingAddress);
                viewModel.AvailableShippingAddresses = Database.GetShippingAddresses(User.Identity.GetUserId());
            }
            else
                return PartialView("_NoShippingAddress");

            return PartialView("_ShippingAddress", viewModel);
        }

        [Authorize]
        public ActionResult NewShippingAddressForm()
        {
            return PartialView("_DefineShippingAddress");
        }

        [HttpPost]
        [Authorize]
        public ActionResult SaveShippingForm(ShippingAddress address)
        {
            ShippingAddressViewModel model = new ShippingAddressViewModel(address);

            if (model.Id == 0)
                model.Id = Database.SaveShippingAddress(address, User.Identity.GetUserId());
            else
                Database.UpdateShippingAddress(address);

            model.AvailableShippingAddresses = Database.GetShippingAddresses(User.Identity.GetUserId());

            return PartialView("_ShippingAddress", model);
        }

        [Authorize]
        public ActionResult ShowShippingAddress(int id)
        {
            ShippingAddress address = Database.GetShippingAddress(id);
            ShippingAddressViewModel model = new ShippingAddressViewModel(address);

            model.AvailableShippingAddresses = Database.GetShippingAddresses(User.Identity.GetUserId());

            Database.SetShippingAddressCurrent(id);

            return PartialView("_ShippingAddress", model);
        }

        [Authorize]
        public ActionResult EditShippingAddress(int id)
        {
            ShippingAddress address = Database.GetShippingAddress(id);
            ShippingAddressViewModel model = new ShippingAddressViewModel(address);

            return PartialView("_DefineShippingAddress", model);
        }

        [Authorize]
        public ActionResult DeleteShippingAddress(int id)
        {
            Database.DeleteShippingAddress(id);

            int count = Database.GetShippingAddressCount(User.Identity.GetUserId());

            if (count == 0)
                return PartialView("_NoShippingAddress");

            List<ShippingAddress> addresses = Database.GetShippingAddresses(User.Identity.GetUserId());

            ShippingAddressViewModel model = new ShippingAddressViewModel(addresses[0]);
            model.AvailableShippingAddresses = addresses;

            Database.SetShippingAddressCurrent(model.Id);

            return PartialView("_ShippingAddress", model);
        }

        [Authorize]
        [HttpPost]
        public string AddToBasket(int itemId, int sizeId, int styleId, int quantity)
        {
            Basket currentBasket = Database.GetCurrentBasket(User.Identity.GetUserId(), false);
            BasketItem newItem = new BasketItem(itemId, sizeId, styleId, quantity);
            newItem.SetPrice(false);

            Database.SaveBasketItem(currentBasket.Id, newItem, 0);

            return "<html>SUCCESS</html>";
        }

        [Authorize]
        [HttpPost]
        public string AddToBasketMulti(string json)
        {
            BasketItem model = new BensJson.JsonDeserializer<BasketItem>().Deserialize(json);
            model.SetPrice(false);

            Basket currentBasket = Database.GetCurrentBasket(User.Identity.GetUserId(), false);
            int id = Database.SaveBasketItem(currentBasket.Id, model, 0);

            foreach (BasketItem item in model.Items)
            {
                item.IsSubItem = true;
                item.SetPrice(true);
                
                Database.SaveBasketItem(id, item, 1);
            }

            return "<html>SUCCESS</html>";
        }

        [Authorize]
        public ActionResult UpdateBasketItem(int id, int itemId, int sizeId, int styleId, int quantity)
        {
            Basket currentBasket = Database.GetCurrentBasket(User.Identity.GetUserId(), true);
            BasketItem thisItem = currentBasket.Items.Where(i => i.Id == id).FirstOrDefault();

            if (thisItem == null)
                return PartialView("Error");

            thisItem.ItemId = itemId;
            thisItem.SizeId = sizeId;
            thisItem.StyleId = styleId;
            thisItem.Quantity = quantity;
            thisItem.SetPrice(false);

            Database.UpdateBasketItem(thisItem);

            ShippingAddress shippingAddress = Database.GetCurrentShippingAddress(User.Identity.GetUserId());
            ShippingAddressViewModel viewModel = null;

            if (shippingAddress != null)
            {
                viewModel = new ShippingAddressViewModel(shippingAddress);
                viewModel.AvailableShippingAddresses = Database.GetShippingAddresses(User.Identity.GetUserId());
            }

            BasketView view = new BasketView(currentBasket, viewModel);

            return PartialView("_SubShoppingBasket", view);
        }

        [Authorize]
        [HttpPost]
        public ActionResult UpdateBasketMulti(string json)
        {
            BasketItem model = new JsonDeserializer<BasketItem>().Deserialize(json);
            model.SetPrice(false);

            Basket currentBasket = Database.GetCurrentBasket(User.Identity.GetUserId(), true);
            Database.UpdateBasketItem(model);

            foreach (BasketItem item in model.Items)
            {
                item.IsSubItem = true;
                item.SetPrice(true);
                Database.UpdateBasketItem(item);
            }

            currentBasket = Database.GetCurrentBasket(User.Identity.GetUserId(), true);

            ShippingAddress shippingAddress = Database.GetCurrentShippingAddress(User.Identity.GetUserId());
            ShippingAddressViewModel viewModel = null;

            if (shippingAddress != null)
            {
                viewModel = new ShippingAddressViewModel(shippingAddress);
                viewModel.AvailableShippingAddresses = Database.GetShippingAddresses(User.Identity.GetUserId());
            }

            BasketView view = new BasketView(currentBasket, viewModel);

            return PartialView("_SubShoppingBasket", view);
        }

        [Authorize]
        public ActionResult DeleteBasketItem(int id)
        {
            Basket currentBasket = Database.GetCurrentBasket(User.Identity.GetUserId(), true);
            BasketItem thisItem = currentBasket.Items.Where(i => i.Id == id).FirstOrDefault();

            if (thisItem == null)
                return PartialView("Error");

            currentBasket.Items.Remove(thisItem);
            Database.DeleteBasketItem(thisItem);

            ShippingAddress shippingAddress = Database.GetCurrentShippingAddress(User.Identity.GetUserId());
            ShippingAddressViewModel viewModel = null;

            if (shippingAddress != null)
            {
                viewModel = new ShippingAddressViewModel(shippingAddress);
                viewModel.AvailableShippingAddresses = Database.GetShippingAddresses(User.Identity.GetUserId());
            }

            BasketView view = new BasketView(currentBasket, viewModel);

            return PartialView("_SubShoppingBasket", view);
        }
        
        public ActionResult ItemDetails(int id)
        {
            Basket basket = Database.GetCurrentBasket(User.Identity.GetUserId(), true);
            BasketItem thisItem = basket.Items.Where(i => i.Id == id).FirstOrDefault();

            if (thisItem == null)
                return PartialView("Error");

            ItemView view = new ItemView(thisItem);

            return PartialView("_ItemDetails", view);
        }

        [Authorize]
        public ActionResult CompleteOrder()
        {
            Database.CreateOrder(User.Identity.GetUserId());

            return View("OrderSuccess");
        }
    }
}