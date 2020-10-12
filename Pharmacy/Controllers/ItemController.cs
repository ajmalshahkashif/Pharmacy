using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Pharmacy.Models;
using Pharmacy.DB;
using System.Web.Script.Serialization;

namespace Pharmacy.Controllers
{
    public class ItemController : BaseController
    {
        public ActionResult AllItems()
        {
            return View();
        }

        public JsonResult GetItemList([DataSourceRequest]DataSourceRequest request)
        {
            var products = context.Items.Select(x => new
            {
                ID = x.ID,
                Barcode = x.Barcode,
                Name = x.Name,
                Description = x.Description,
                Shelf = x.Shelf,
                PurchasePrice = x.PurchasePrice,
                SalePrice = x.SalePrice,
                PiecesPerPack = x.PiecesPerPack,
                SalePricePerPiece = x.SalePricePerPiece,
                OtherBonus = x.OtherBonus,
                ManufacturerID = x.ManufacturerID,
                ItemTypeID = x.ItemTypeID,
                Packing = x.Packing,
                PackSize = x.PackSize,
                GenericName = x.GenericName,
                PurchasePercentage = x.PurchasePercentage,
                SalePercentage = x.SalePercentage,
                TotalStock = x.TotalStock,
                LooseQuantitySold = x.LooseQuantitySold,
                ItemTypeName = x.ItemType.Name,
                LowStockQuantity = x.LowStockQuantity,
                CriticalLowStockQuantity = x.CriticalLowStockQuantity
            }).ToList();
            return this.Json(products.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult UpdateItem([DataSourceRequest] DataSourceRequest request, Item item)
        {
            try
            {
                if (item != null && ModelState.IsValid)
                {
                    var itemToUpdate = context.Items.Where(x => x.ID == item.ID).FirstOrDefault();
                    itemToUpdate.Barcode = item.Barcode;
                    itemToUpdate.Name = item.Name;
                    itemToUpdate.Description = item.Description;
                    itemToUpdate.Shelf = item.Shelf;
                    itemToUpdate.PurchasePrice = item.PurchasePrice;
                    itemToUpdate.SalePrice = item.SalePrice;
                    itemToUpdate.PiecesPerPack = item.PiecesPerPack;
                    itemToUpdate.SalePricePerPiece = item.SalePricePerPiece;
                    itemToUpdate.OtherBonus = item.OtherBonus;
                    itemToUpdate.ManufacturerID = item.ManufacturerID;
                    itemToUpdate.ItemTypeID = item.ItemTypeID;
                    itemToUpdate.Packing = item.Packing;
                    itemToUpdate.PackSize = item.PackSize;
                    itemToUpdate.GenericName = item.GenericName;
                    itemToUpdate.PurchasePercentage = item.PurchasePercentage;
                    itemToUpdate.SalePercentage = item.SalePercentage;
                    itemToUpdate.TotalStock = item.TotalStock;
                    itemToUpdate.PiecesPerPack = item.PiecesPerPack;
                    itemToUpdate.LowStockQuantity = item.LowStockQuantity;
                    itemToUpdate.CriticalLowStockQuantity = item.CriticalLowStockQuantity;

                    context.SaveChanges();
                }

                return Json("Success");
            }
            catch (Exception ex)
            {
                return Json(ex.Message);
            }

        }

        public JsonResult DeleteItem([DataSourceRequest] DataSourceRequest request, Item item)
        {
            if (item != null)
            {
                var itemToDelete = context.Items.Where(x => x.ID == item.ID).FirstOrDefault();
                context.Items.Remove(itemToDelete);
                context.SaveChanges();
            }
            return Json("Item with ID = " + item.ID + " And name = " + item.Name + " is deleted");
        }

        public ActionResult AddItem()
        {
            return View();
        }

        //[HttpGet]
        //public ActionResult AddItem(int? id)
        //{
        //    JavaScriptSerializer JsonConvert = new JavaScriptSerializer();
        //    var tt = context.Items.Where(x => x.ID == id).Select(x => new
        //    {
        //        ID = x.ID,
        //        Name = x.Name,
        //        //ItemTypeID = x.ItemTypeID,
        //        ItemTypeID = x.ItemType.ID,
        //        ManufacturerID = x.ManufacturerID,
        //        Shelf = x.Shelf,
        //        PurchasePercentage = x.PurchasePercentage,
        //        SalePercentage = x.SalePercentage,
        //        SalePrice = x.SalePrice,
        //        PurchasePrice = x.PurchasePrice,
        //        Packing = x.Packing,
        //        GenericName = x.GenericName,
        //        OtherBonus = x.OtherBonus
        //    }).FirstOrDefault();

        //    string serializeString = JsonConvert.Serialize(tt);
        //    ItemValidation item = JsonConvert.Deserialize<ItemValidation>(serializeString);

        //    return View(item);
        //}

        [HttpPost]
        public ActionResult AddItem(ItemValidation item)
        {
            Item obj = new Item();

            obj.Barcode = item.Barcode;
            obj.Name = item.Name;
            obj.Description = item.Description;
            obj.Shelf = item.Shelf;
            obj.PurchasePrice = item.PurchasePrice;
            obj.SalePrice = item.SalePrice;
            obj.PiecesPerPack = item.PiecesPerPack;
            obj.SalePricePerPiece = item.SalePricePerPiece;
            obj.OtherBonus = item.OtherBonus;
            obj.ManufacturerID = item.ManufacturerID;
            obj.ItemTypeID = item.ItemTypeID;
            obj.PurchasePercentage = item.PurchasePercentage;
            obj.SalePercentage = item.SalePercentage;
            obj.TotalStock = 0;
            obj.LooseQuantitySold = 0;
            obj.PiecesPerPack = item.PiecesPerPack;

            context.Items.Add(obj);
            context.SaveChanges();
            ModelState.Clear();
            return View();
        }

        public ActionResult AlarmingStockItem(string ItemIntensity)
        {
            ViewBag.ItemIntensity = ItemIntensity;
            return View();
        }

        public JsonResult GetAlarmingItemList([DataSourceRequest]DataSourceRequest request, string ItemIntensity)
        {
            List<Item> products = new List<Item>();
            IEnumerable<Item> products2 = new List<Item>();

            if (ItemIntensity == "LowStockItem")
            {
                products = context.Items.Select(x => new
                {
                    ID = x.ID,
                    Name = x.Name,
                    TotalStock = x.TotalStock,
                    LooseQuantitySold = x.LooseQuantitySold,
                    LowStockQuantity = x.LowStockQuantity,
                    CriticalLowStockQuantity = x.CriticalLowStockQuantity

                }).ToList().Select(p => new Item()
                {
                    ID = p.ID,
                    Name = p.Name,
                    TotalStock = p.TotalStock,
                    LooseQuantitySold = p.LooseQuantitySold,
                    LowStockQuantity = p.LowStockQuantity,
                    CriticalLowStockQuantity = p.CriticalLowStockQuantity
                }).ToList().Where(x => x.TotalStock <= x.LowStockQuantity).ToList();
            }

            else if (ItemIntensity == "CriticalLowStockItem")
            {
                products = context.Items.Select(x => new
                {
                    ID = x.ID,
                    Name = x.Name,
                    TotalStock = x.TotalStock,
                    LooseQuantitySold = x.LooseQuantitySold,
                    LowStockQuantity = x.LowStockQuantity,
                    CriticalLowStockQuantity = x.CriticalLowStockQuantity

                }).ToList().Select(p => new Item()
                {
                    ID = p.ID,
                    Name = p.Name,
                    TotalStock = p.TotalStock,
                    LooseQuantitySold = p.LooseQuantitySold,
                    LowStockQuantity = p.LowStockQuantity,
                    CriticalLowStockQuantity = p.CriticalLowStockQuantity
                }).ToList().Where(x => x.TotalStock <= x.CriticalLowStockQuantity).ToList();
            }

            return this.Json(products.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
        }

        public JsonResult AlarmingItemUpdate([DataSourceRequest] DataSourceRequest request, Item item)
        {
            try
            {
                if (item != null && ModelState.IsValid)
                {
                    var itemToUpdate = context.Items.Where(x => x.ID == item.ID).FirstOrDefault();
                    itemToUpdate.LowStockQuantity = item.LowStockQuantity;
                    itemToUpdate.CriticalLowStockQuantity = item.CriticalLowStockQuantity;

                    context.SaveChanges();
                }

                return Json("Success");
            }
            catch (Exception ex)
            {
                return Json(ex.Message);
            }

        }

        #region DDL population

        public JsonResult ddlItemType()
        {
            var itemTypeList = context.ItemTypes.Select(x => new { ID = x.ID, Name = x.Name }).ToList();
            return Json(itemTypeList, JsonRequestBehavior.AllowGet);
        }

        public JsonResult ddlCompanies()
        {
            var companiesList = context.Companies.Select(x => new { ID = x.ID, Name = x.Name }).ToList();
            return Json(companiesList, JsonRequestBehavior.AllowGet);
        }

        #endregion

    }
}
