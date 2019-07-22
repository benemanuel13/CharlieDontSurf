using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using CharlieDontSurf.Models.Inventory;
using CharlieDontSurf.Models.Shopping;
using CharlieDontSurf.Infrastructure;

namespace CharlieDontSurf.Controllers
{
    public class ShoppingController : Controller
    {
        // GET: Shopping
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult ShoppingMain()
        {
            ShoppingModel model = new ShoppingModel();
            model.ItemTypes = Database.GetItemTypes();
            model.Items = Database.GetItems(model.ItemTypes.First().Id);

            return PartialView("_ShoppingMain", model);
        }

        public ActionResult ItemList(int typeId)
        {
            List<Item> items = Database.GetItems(typeId);

            return PartialView("_ItemList", items);
        }
    }
}