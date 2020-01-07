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

        public JsonResult SupplierList(string Prefix)
        {
            var SupplierList = context.Suppliers.Where(x => x.Name.StartsWith(Prefix)).Select(x => new { Name = x.Name, ID = x.ID }).ToList();
            return Json(SupplierList, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetItemByID(int? itemID)
        {
            var item = context.Items.Where(x => x.ID == itemID).Select(x => new
            {
                ID = x.ID,
                Name = x.Name,
                PurchasePrice = x.PurchasePrice,
                SalePercentage = x.SalePercentage,
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
            newPurchase.AmountPaid = purchase.Select(x => x.AmountPaid).FirstOrDefault();
            newPurchase.Arears = purchase.Select(x => x.Arears).FirstOrDefault();
            newPurchase.SupplierID = purchase.Select(x => x.SupplierID).FirstOrDefault();
            newPurchase.DateOfSupply = purchase.Select(x => x.DateOfSupply).FirstOrDefault();

            context.Purchases.Add(newPurchase);
            context.SaveChanges();

            foreach (var n in purchase)
            {
                PurchaseItem purchaseItem = new PurchaseItem();

                var itemID = context.Items.Where(x => x.Name == n.Name).Select(x => x.ID).FirstOrDefault();

                purchaseItem.PurchaseID = newPurchase.ID;
                purchaseItem.ItemID = itemID;
                purchaseItem.PurchasePrice = n.PurchasePrice;
                purchaseItem.PurchasePercentage = n.PurchasePercentage;
                purchaseItem.SalePercentage = n.SalePercentage;
                purchaseItem.Quantity = n.Quantity;

                context.PurchaseItems.Add(purchaseItem);
                context.SaveChanges();

                var Item = context.Items.Where(x => x.ID == itemID).FirstOrDefault();

                var RetailPrice = n.PurchasePrice + (n.PurchasePercentage * n.PurchasePrice / 100);

                Item.PurchasePrice = n.PurchasePrice;
                Item.PurchasePercentage = n.PurchasePercentage;
                Item.SalePercentage = n.SalePercentage;
                Item.SalePrice = RetailPrice;
                Item.TotalStock += n.Quantity;
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