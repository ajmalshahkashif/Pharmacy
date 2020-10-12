using Pharmacy.DB;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Pharmacy.Controllers
{
    public class BaseController : Controller
    {
        public PharmacyEntities context = new PharmacyEntities();

        public BaseController()
        {
            var Total = context.Sales.Where(x => x.DateOfSale.Value.Year == DateTime.Today.Year &&
            x.DateOfSale.Value.Month == DateTime.Today.Month &&
            x.DateOfSale.Value.Day == DateTime.Today.Day)
                .Sum(x => x.TotalAfterPercentage);

            ViewBag.totalSales = Total;

            var TotalLowStockItems = context.Items.Where(x => x.TotalStock <= x.LowStockQuantity).Count();
            ViewBag.TotalLowStockItems = TotalLowStockItems;

            var TotalCriticalLowStockItems = context.Items.Where(x => x.TotalStock <= x.CriticalLowStockQuantity).Count();
            ViewBag.TotalCriticalLowStockItems = TotalCriticalLowStockItems;

            //decimal? total = context.Sales.Where(x => x.DateOfSale.Value.Date == DateTime.Today).Sum(x => x.TotalAfterPercentage);
            //string connectionString = @"data source=DESKTOP-OL4CIK1\\SQLEXPRESS;initial catalog=Pharmacy;integrated security=True;";
            //string queryString = "select sum(TotalAfterPercentage) as [Today's total sale] from Sale where cast( DateOfSale as date) = cast(getdate() as date)";

            //using (SqlConnection conn = new SqlConnection(connectionString))
            //{
            //    using (SqlCommand cmd = new SqlCommand(queryString, conn))
            //    {
            //        conn.Open();
            //        int totalSales = cmd.ExecuteNonQuery();
            //        //conn.Close();
            //    }

            //}

        }

    }
}