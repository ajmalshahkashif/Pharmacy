using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Pharmacy.Models;
using Pharmacy.DB;

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
                TotalStock = x.TotalStock
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

        [HttpPost]
        public ActionResult AddItem(ItemValidation item)
        {
            Item obj = new Item();

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

            context.Items.Add(obj);
            context.SaveChanges();
            ModelState.Clear();
            return View();
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
