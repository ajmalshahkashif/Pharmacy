using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Pharmacy.Controllers
{
    public class CompanyController : BaseController
    {
        // GET: Company
        public ActionResult AllCompanies()
        {
            var Companies = context.Companies.ToList();

            return View(Companies);
        }

        public JsonResult Get([DataSourceRequest]DataSourceRequest request)
        {
            var companies = context.Companies.ToList();
            return this.Json(companies.ToDataSourceResult(request));
        }

        public ActionResult AddCompany()
        {

            return View();
        }

        //[HttpPost]
        //public ActionResult AddCompany(ItemValidation item)
        //{

        //    Item obj = new Item();

        //    obj.Name = item.Name;
        //    obj.Shelf = item.Shelf;

        //    context.Items.Add(obj);
        //    context.SaveChanges();
        //    return View();
        //}

    }
}