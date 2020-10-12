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
    public class SaleReturnController : BaseController
    {
        public ActionResult AllSaleReturn()
        {
            var saleReturnList = context.SaleReturns.ToList();

            return View(saleReturnList);
        }

        [HttpGet]
        public ActionResult AddSaleReturn()
        {
            return View();
        }

        [HttpPost]
        public ActionResult AddSaleReturn(string Prefix)
        {
            return View();
        }

        public JsonResult MedicineList(string Prefix)
        {
            var AllItems = context.Items.Select(x => new { ID = x.ID, Name = x.Name }).ToList();
            var ItemList = AllItems.Where(x => x.Name.StartsWith(Prefix, StringComparison.OrdinalIgnoreCase)).Select(x => new { Name = x.Name, ID = x.ID }).ToList();
            //var ItemList = AllItems.Where(x => x.Name.StartsWith(Prefix, StringComparison.OrdinalIgnoreCase)).Select(x => new { Name = x.Name, ID = x.ID }).ToList();

            //var ItemList = context.Items.Where(x => x.Name.Contains(Prefix)).Select(x => new { Name = x.Name, ID = x.ID }).ToList();

            return Json(ItemList, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetItemByID(int? itemID)
        {
            var item = context.Items.Where(x => x.ID == itemID).Select(x => new
            {
                ID = x.ID,
                Name = x.Name,
                SalePrice = x.SalePrice,
                SalePercentage = x.SalePercentage,
                TotalStock = x.TotalStock,
                ItemTypeID = x.ItemTypeID,
                PiecesPerPack = x.PiecesPerPack
            }).FirstOrDefault();
            return Json(item, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult SaveSaleItems(List<SaleReturnValidation> saleReturn)
        {
            SaleReturn newSaleReturn = new SaleReturn();

            newSaleReturn.TotalBeforePercentage = saleReturn.Select(x => x.TotalBeforePercentage).FirstOrDefault();

            decimal? totalAfterPercentage = saleReturn.Select(x => x.TotalAfterPercentage).FirstOrDefault();
            newSaleReturn.TotalAfterPercentage = saleReturn.Select(x => x.TotalAfterPercentage).FirstOrDefault();
            newSaleReturn.SpecialDiscount = saleReturn.Select(x => x.SpecialDiscount).FirstOrDefault();
            newSaleReturn.AmountPaid = saleReturn.Select(x => x.AmountPaid).FirstOrDefault();
            newSaleReturn.Arears = saleReturn.Select(x => x.Arears).FirstOrDefault();
            newSaleReturn.DateOfSaleReturn = DateTime.Now;

            context.SaleReturns.Add(newSaleReturn);
            context.SaveChanges();

            foreach (var n in saleReturn)
            {
                SaleReturnItem saleReturnItem = new SaleReturnItem();

                var itemID = context.Items.Where(x => x.Name == n.Name).Select(x => x.ID).FirstOrDefault();

                saleReturnItem.SaleReturnID = newSaleReturn.ID;
                saleReturnItem.ItemID = itemID;
                saleReturnItem.SalePercentage = n.SalePercentage;
                saleReturnItem.Quantity = n.Quantity;
                saleReturnItem.LoosePack = n.LoosePack;

                context.SaleReturnItems.Add(saleReturnItem);
                context.SaveChanges();

                var Item = context.Items.Where(x => x.ID == itemID).FirstOrDefault();

                if (n.LoosePack == "P")
                    Item.TotalStock += n.Quantity;
                else if (n.LoosePack == "L")
                {
                    Item.LooseQuantitySold += n.Quantity;
                    if (Item.LooseQuantitySold >= n.PiecesPerPack)
                    {
                        int? packReturned = Item.LooseQuantitySold / n.PiecesPerPack;
                        int? looseQuantityleftAfterPackomission = Item.LooseQuantitySold % n.PiecesPerPack;
                        Item.TotalStock += packReturned;
                        Item.LooseQuantitySold = looseQuantityleftAfterPackomission;

                    }
                }
                //TODO: remove last line. just to save saleprice at run time
                Item.SalePrice = n.SalePrice;
                context.SaveChanges();
            }

            return Json(true);
        }

        public JsonResult GetSaleList([DataSourceRequest]DataSourceRequest request)
        {
            var saleReturnList = context.SaleReturns.Select(x => new
            {
                ID = x.ID,
                SpecialDiscount = x.SpecialDiscount,
                TotalAfterPercentage = x.TotalAfterPercentage,
                TotalBeforePercentage = x.TotalBeforePercentage,
                AmountPaid = x.AmountPaid,
                DateOfSale = x.DateOfSaleReturn,
                Arears = x.Arears
            }).ToList();
            return this.Json(saleReturnList.ToDataSourceResult(request));
        }

        [HttpPost]
        public JsonResult GetItemTypeName(int? ItemTypeID)
        {
            var item = context.ItemTypes.Where(x => x.ID == ItemTypeID).Select(x => new
            {
                ID = x.ID,
                Name = x.Name,
                isActive = x.isActive

            }).FirstOrDefault();
            return Json(item, JsonRequestBehavior.AllowGet);
        }

    }
}