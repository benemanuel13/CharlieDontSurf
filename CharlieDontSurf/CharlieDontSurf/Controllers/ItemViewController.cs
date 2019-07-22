using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using CharlieDontSurf.Models.Basket;
using CharlieDontSurf.Models.Inventory;
using CharlieDontSurf.Infrastructure;

namespace CharlieDontSurf.Controllers
{
    public class ItemViewController : Controller
    {
        // GET: ItemView
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult ItemView(int id)
        {
            Item thisItem = Database.GetItem(id);

            return View(thisItem);
        }

        public ActionResult ItemDetails(int id)
        {
            Item thisItem = Database.GetItem(id, true);
            ItemView view = new ItemView(thisItem);
            
            return PartialView("_ItemDetails", view);
        }
    }
}