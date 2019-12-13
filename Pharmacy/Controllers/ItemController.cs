﻿using Kendo.Mvc.Extensions;
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
                Name = x.Name,
                ItemTypeID = x.ItemTypeID,
                ManufacturerID = x.ManufacturerID,
                Shelf = x.Shelf,
                PurchasePrice = x.PurchasePrice,
                SalePrice = x.SalePrice,
                PiecesPerPack = x.PiecesPerPack,
                SalePricePerPiece = x.SalePricePerPiece,
                Description = x.Description
            }).ToList();
            return this.Json(products.ToDataSourceResult(request), JsonRequestBehavior.AllowGet);
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
