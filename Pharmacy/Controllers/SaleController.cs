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
            //Note : you can bind same list from database
            var ItemList = context.Items.Select(x => new { ID = x.ID, Name = x.Name }).ToList();

            //Searching records from list using LINQ query  
            //var CityList1 = ItemList.Where(x => x.Name.StartsWith(Prefix)).Select(x => x.Name).ToList();
            var CityList = ItemList.Where(x => x.Name.StartsWith(Prefix)).Select(x => new { Name = x.Name, ID = x.ID });

            return Json(CityList, JsonRequestBehavior.AllowGet);
        }
    }
}