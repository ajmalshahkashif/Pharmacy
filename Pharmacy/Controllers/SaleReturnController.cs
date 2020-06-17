using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Pharmacy.Controllers
{
    public class SaleReturnController : Controller
    {
        // GET: SaleReturn
        public ActionResult AddSaleReturn()
        {
            return View();
        }
    }
}