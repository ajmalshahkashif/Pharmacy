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
            var products = context.Items.ToList();

            return View(products);
        }

        public JsonResult Get([DataSourceRequest]DataSourceRequest request)
        {
            var products = context.Items.ToList();
            return this.Json(products.ToDataSourceResult(request));
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
            obj.Shelf = item.Shelf;

            context.Items.Add(obj);
            context.SaveChanges();
            return View();
        }


        public JsonResult ReturnJSONDataToAJax() //It will be fired from Jquery ajax call  
        {
            var jsonData = context.ItemTypes.ToList();
            return Json(jsonData, JsonRequestBehavior.AllowGet);
        }
    }
}
