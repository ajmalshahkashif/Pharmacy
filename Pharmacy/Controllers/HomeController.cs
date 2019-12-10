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
    public class HomeController : BaseController
    {
       
        public ActionResult Index()
        {
            var products = context.Items.ToList();

            return View(products);
        }

        public JsonResult Get([DataSourceRequest]DataSourceRequest request)
        {
            var products = context.Items.ToList();
            return this.Json(products.ToDataSourceResult(request));
        }
    }
}
