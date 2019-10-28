using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Pharmacy.Controllers
{
    public class ItemController : BaseController
    {
        // GET: Item
        public ActionResult AllItems()
        {
            var products = _context.Items.ToList();

            return View(products);
        }

        public JsonResult Get([DataSourceRequest]DataSourceRequest request)
        {
            var products = _context.Items.ToList();
            return this.Json(products.ToDataSourceResult(request));
        }

        public ActionResult AddItem()
        {

            return View();
        }

    }
}