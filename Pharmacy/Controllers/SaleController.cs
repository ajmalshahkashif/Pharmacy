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
            var item = context.Items.Where(x => x.ID == itemID).FirstOrDefault();
            return Json(item, JsonRequestBehavior.AllowGet);
        }
    }
}