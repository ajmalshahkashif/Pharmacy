using Pharmacy.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Pharmacy.Controllers
{
    public class BaseController : Controller
    {
       public PharmacyEntities _context = new PharmacyEntities();

    }
}