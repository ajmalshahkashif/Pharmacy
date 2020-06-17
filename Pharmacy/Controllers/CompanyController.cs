using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Pharmacy.Models;
using Pharmacy.DB;
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
            return View();
        }

        public JsonResult Get([DataSourceRequest]DataSourceRequest request)
        {
            var ldsf = context.Companies.ToList();
            var companies = context.Companies.Select(x => new
            {
                ID = x.ID,
                Name = x.Name,
                Logo = x.Logo,
                Address = x.Address,
                Description = x.Description,
                isActive = x.isActive
            }).ToList();
            return this.Json(companies.ToDataSourceResult(request));
        }

        public ActionResult AddCompany()
        {

            return View();
        }

        [HttpPost]
        public ActionResult AddCompany(CompanyValidation company)
        {

            Company obj = new Company();

            obj.Name = company.Name;
            obj.Logo = company.Logo;
            obj.Address = company.Address;
            obj.Description = company.Description;
            obj.isActive = company.isActive;

            context.Companies.Add(obj);
            context.SaveChanges();
            ModelState.Clear();
            return View();
        }



        #region DDL population




        #endregion
    }
}