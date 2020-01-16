using Pharmacy.DB;
using Pharmacy.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Pharmacy.Controllers
{
    public class SaleController : BaseController
    {

        [HttpGet]
        public ActionResult AddSale()
        {
            return View();
        }

        [HttpPost]
        public ActionResult AddSale(string Prefix)
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
                SalePrice = x.SalePrice,
                SalePercentage = x.SalePercentage,
                TotalStock = x.TotalStock
            }).FirstOrDefault();
            return Json(item, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult SaveSaleItems(List<SaleValidation> sale)
        {
            Sale newSale = new Sale();

            newSale.TotalBeforePercentage = sale.Select(x => x.TotalBeforePercentage).FirstOrDefault();
            newSale.TotalAfterPercentage = sale.Select(x => x.TotalAfterPercentage).FirstOrDefault();
            newSale.AmountPaid = sale.Select(x => x.AmountPaid).FirstOrDefault();
            newSale.Arears = sale.Select(x => x.Arears).FirstOrDefault();

            context.Sales.Add(newSale);
            context.SaveChanges();

            foreach (var n in sale)
            {
                SaleItem saleItem = new SaleItem();

                var itemID = context.Items.Where(x => x.Name == n.Name).Select(x => x.ID).FirstOrDefault();

                saleItem.SaleID = newSale.ID;
                saleItem.ItemID = itemID;
                saleItem.SalePercentage = n.SalePercentage;
                saleItem.Quantity = n.Quantity;

                context.SaleItems.Add(saleItem);
                context.SaveChanges();
            }

            return Json(true);
        }
    }
}