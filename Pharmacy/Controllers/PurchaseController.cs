using Pharmacy.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Pharmacy.Controllers
{
    public class PurchaseController : BaseController
    {
        public ActionResult AllPurchases()
        {
            return View();
        }

        public ActionResult AddPurchase()
        {
            return View();
        }



        public JsonResult MedicineList(string Prefix)
        {
            var ItemList = context.Items.Select(x => new { ID = x.ID, Name = x.Name }).ToList();


            var CityList = ItemList.Where(x => x.Name.StartsWith(Prefix)).Select(x => new { Name = x.Name, ID = x.ID }).ToList();

            return Json(CityList, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetItemByID(int? itemID)
        {
            var item = context.Items.Where(x => x.ID == itemID).Select(x => new
            {
                ID = x.ID,
                Name = x.Name,
                PurchasePrice = x.PurchasePrice,
                PurchasePercentage = x.PurchasePercentage,
                TotalStock = x.TotalStock
            }).FirstOrDefault();
            return Json(item, JsonRequestBehavior.AllowGet);
        }



        public RedirectResult SavePurchasedItems(List<List<string>> purchaseArray)
        {

            return Redirect("AllPurchases");
        }


    }
}