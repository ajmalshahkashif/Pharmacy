using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Microsoft.Reporting.WebForms;
using Pharmacy.DB;
using Pharmacy.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Printing;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace Pharmacy.Controllers
{
    public class PurchaseController : BaseController
    {
        private static List<Stream> m_streams;
        private static int m_currentPageIndex = 0;

        public ActionResult AllPurchases()
        {
            var purchaseList = context.Purchases.ToList();
            return View(purchaseList);
        }

        public ActionResult AddPurchase()
        {
            //ModelState.Clear();
            return View();
        }

        public JsonResult MedicineList(string Prefix)
        {
            var AllItems = context.Items.Select(x => new { ID = x.ID, Name = x.Name }).ToList();
            var ItemList = AllItems.Where(x => x.Name.StartsWith(Prefix, StringComparison.OrdinalIgnoreCase)).Select(x => new { Name = x.Name, ID = x.ID }).ToList();
            //var ItemList = AllItems.Where(x => x.Name.StartsWith(Prefix, StringComparison.OrdinalIgnoreCase)).Select(x => new { Name = x.Name, ID = x.ID }).ToList();

            //var ItemList = context.Items.Where(x => x.Name.Contains(Prefix)).Select(x => new { Name = x.Name, ID = x.ID }).ToList();

            return Json(ItemList, JsonRequestBehavior.AllowGet);

        }

        public JsonResult SupplierList(string Prefix)
        {
            var SupplierList = context.Suppliers.Where(x => x.Name.StartsWith(Prefix)).Select(x => new { Name = x.Name, ID = x.ID }).ToList();
            return Json(SupplierList, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetItemByID(int? itemID)
        {
            var item = context.Items.Where(x => x.ID == itemID).Select(x => new
            {
                ID = x.ID,
                Name = x.Name,
                PurchasePrice = x.PurchasePrice,
                SalePrice = x.SalePrice,
                SalePercentage = x.SalePercentage,
                PurchasePercentage = x.PurchasePercentage,
                TotalStock = x.TotalStock
            }).FirstOrDefault();
            return Json(item, JsonRequestBehavior.AllowGet);
        }

        #region Print Code
        public ActionResult Report(string id)
        {
            string sql = "select * from Purchase";
            SqlConnection con = new SqlConnection("data source=DESKTOP-04JEUL0\\AJMALINSTANCE;initial catalog=Pharmacy;user id=sa;password=50239;MultipleActiveResultSets=True;App=EntityFramework");
            var dt = new DataTable();
            var ds = new DataSet();

            List<PurchaseItem> list = new List<PurchaseItem>();
            list.Add(new PurchaseItem { PurchaseID = 1, ItemID = 1, Quantity = 3 });
            list.Add(new PurchaseItem { PurchaseID = 2, ItemID = 1, Quantity = 4 });
            list.Add(new PurchaseItem { PurchaseID = 3, ItemID = 2, Quantity = 5 });

            var dt2 = new DataTable();
            dt2.Columns.Add("PurchaseID");
            dt2.Columns.Add("ItemID");
            dt2.Columns.Add("Quantity");
            foreach (var item in list)
            {
                var row = dt2.NewRow();
                row["PurchaseID"] = item.PurchaseID;
                row["ItemID"] = item.ItemID;
                row["Quantity"] = item.Quantity;
                dt2.Rows.Add(row);
            }
            con.Open();
            SqlDataAdapter adpt = new SqlDataAdapter(sql, con);
            adpt.Fill(dt);
            con.Close();

            LocalReport report = new LocalReport();
            string fullpath = Path.GetDirectoryName("Report/rdpProductMemo.rdlc");

            report.ReportPath = "Report/rdpProductMemo.rdlc";
            report.DataSources.Add(new ReportDataSource("dsName1", dt2));

            PrintToPrinter(report);
            return View();
        }

        //public DataTable GetDataTable(string sql)
        //{
        //    try
        //    {
        //        SqlConnection con = new SqlConnection("data source=DESKTOP-04JEUL0\\AJMALINSTANCE;initial catalog=Pharmacy;user id=sa;password=50239;MultipleActiveResultSets=True;App=EntityFramework");
        //        var dt = new DataTable();
        //        con.Open();
        //        SqlDataAdapter adpt = new SqlDataAdapter(sql, con);
        //        adpt.Fill(dt);
        //        con.Close();
        //        return dt;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new Exception(ex.Message);
        //    }
        //}

        public static void PrintToPrinter(LocalReport report)
        {
            Export(report);

        }

        public static void Export(LocalReport report, bool print = true)
        {
            string deviceInfo =
             @"<DeviceInfo>
                <OutputFormat>EMF</OutputFormat>
                <PageWidth>34cm</PageWidth>
                <PageHeight>29.7cm</PageHeight>
                <MarginTop>1cm</MarginTop>
                <MarginLeft>0.4cm</MarginLeft>
                <MarginRight>0.4cm</MarginRight>
                <MarginBottom>1cm</MarginBottom>
            </DeviceInfo>";
            Warning[] warnings;
            m_streams = new List<Stream>();
            report.Render("Image", deviceInfo, CreateStream, out warnings);
            foreach (Stream stream in m_streams)
                stream.Position = 0;

            if (print)
            {
                Print();
            }
        }

        public static void Print()
        {
            if (m_streams == null || m_streams.Count == 0)
                throw new Exception("Error: no stream to print.");
            PrintDocument printDoc = new PrintDocument();
            if (!printDoc.PrinterSettings.IsValid)
            {
                throw new Exception("Error: cannot find the default printer.");
            }
            else
            {
                printDoc.PrintPage += new PrintPageEventHandler(PrintPage);
                m_currentPageIndex = 0;
                printDoc.Print();
            }
        }

        public static Stream CreateStream(string name, string fileNameExtension, Encoding encoding, string mimeType, bool willSeek)
        {
            Stream stream = new MemoryStream();
            m_streams.Add(stream);
            return stream;
        }

        public static void PrintPage(object sender, PrintPageEventArgs ev)
        {
            Metafile pageImage = new
               Metafile(m_streams[m_currentPageIndex]);

            // Adjust rectangular area with printer margins.
            Rectangle adjustedRect = new Rectangle(
                ev.PageBounds.Left - (int)ev.PageSettings.HardMarginX,
                ev.PageBounds.Top - (int)ev.PageSettings.HardMarginY,
                ev.PageBounds.Width,
                ev.PageBounds.Height);

            // Draw a white background for the report
            ev.Graphics.FillRectangle(Brushes.White, adjustedRect);

            // Draw the report content
            ev.Graphics.DrawImage(pageImage, adjustedRect);

            // Prepare for the next page. Make sure we haven't hit the end.
            m_currentPageIndex++;
            ev.HasMorePages = (m_currentPageIndex < m_streams.Count);
        }

        public static void DisposePrint()
        {
            if (m_streams != null)
            {
                foreach (Stream stream in m_streams)
                    stream.Close();
                m_streams = null;
            }
        }

        #endregion

        public JsonResult GetItemByBarCode(string barCode)
        {
            var item = context.Items.Where(x => x.Barcode == barCode).Select(x => new
            {
                ID = x.ID,
                Barcode = x.Barcode,
                Name = x.Name,
                PurchasePrice = x.PurchasePrice,
                SalePrice = x.SalePrice,
                SalePercentage = x.SalePercentage,
                PurchasePercentage = x.PurchasePercentage,
                TotalStock = x.TotalStock
            }).FirstOrDefault();
            return Json(item, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public JsonResult SavePurchasedItems(List<PurchaseValidation> purchase)
        {
            Purchase newPurchase = new Purchase();

            newPurchase.NetTotal = purchase.Select(x => x.NetTotal).FirstOrDefault();
            newPurchase.AmountPaid = purchase.Select(x => x.AmountPaid).FirstOrDefault();
            newPurchase.Arears = purchase.Select(x => x.Arears).FirstOrDefault();
            newPurchase.SupplierID = purchase.Select(x => x.SupplierID).FirstOrDefault();
            newPurchase.DateOfSupply = purchase.Select(x => x.DateOfSupply).FirstOrDefault();

            context.Purchases.Add(newPurchase);
            context.SaveChanges();

            foreach (var n in purchase)
            {
                PurchaseItem purchaseItem = new PurchaseItem();

                var itemID = context.Items.Where(x => x.Name == n.Name).Select(x => x.ID).FirstOrDefault();

                purchaseItem.PurchaseID = newPurchase.ID;
                purchaseItem.ItemID = itemID;
                purchaseItem.PurchasePrice = n.PurchasePrice;
                purchaseItem.SalePrice = n.SalePrice;
                purchaseItem.PurchasePercentage = n.PurchasePercentage;
                purchaseItem.SalePercentage = n.SalePercentage;
                purchaseItem.Quantity = n.Quantity;

                context.PurchaseItems.Add(purchaseItem);
                context.SaveChanges();

                var Item = context.Items.Where(x => x.ID == itemID).FirstOrDefault();

                var RetailPrice = n.PurchasePrice + (n.PurchasePercentage * n.PurchasePrice / 100);

                Item.PurchasePrice = n.PurchasePrice;
                Item.SalePrice = n.SalePrice;
                Item.PurchasePercentage = n.PurchasePercentage;
                Item.SalePercentage = n.SalePercentage;
                //Item.SalePrice = RetailPrice;
                Item.SalePricePerPiece = RetailPrice / Item.PackSize;
                Item.TotalStock = Item.TotalStock + n.Quantity;
                //Item.TotalStock += n.Quantity;
                context.SaveChanges();

            }

            return Json(true);
        }

        public JsonResult GetPurchaseList([DataSourceRequest]DataSourceRequest request)
        {
            var purchaseList = context.Purchases.Select(x => new
            {
                ID = x.ID,
                Name = x.Supplier.Name,
                Description = x.DateOfSupply,
                isActive = x.NetTotal
            }).ToList();
            return this.Json(purchaseList.ToDataSourceResult(request));
        }
    }
}