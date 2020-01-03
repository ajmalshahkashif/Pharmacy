using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Pharmacy.DB;
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
            var purchaseList = context.Purchases.ToList();
            return View(purchaseList);
        }

        public ActionResult AddPurchase()
        {
            //ModelState.Clear();
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

        [HttpPost]
        public JsonResult SavePurchasedItems(List<PurchaseValidation> purchase)

        {
            Purchase newPurchase = new Purchase();
            newPurchase.NetTotal = purchase.Select(x => x.NetTotal).FirstOrDefault();
            context.Purchases.Add(newPurchase);
            context.SaveChanges();

            foreach (var n in purchase)
            {
                PurchaseItem purchaseItem = new PurchaseItem();

                var itemID = context.Items.Where(x => x.Name == n.Name).Select(x => x.ID).FirstOrDefault();

                purchaseItem.PurchaseID = newPurchase.ID;
                purchaseItem.ItemID = itemID;
                purchaseItem.PurchasePrice = n.PurchasePrice;
                purchaseItem.Percentage = n.PurchasePercentage;
                purchaseItem.Quantity = n.Quantity;

                context.PurchaseItems.Add(purchaseItem);
                context.SaveChanges();
            }

            return Json(true);
        }

        public JsonResult GetPurchaseList([DataSourceRequest]DataSourceRequest request)
        {
            var purchaseList = context.Purchases.Select(x => new
            {
                ID = x.ID,
                Name = x.Supplier.Name,
                Description = x.DateOfSupply,
                isActive = x.NetTotal
            }).ToList();
            return this.Json(purchaseList.ToDataSourceResult(request));
        }
    }
}