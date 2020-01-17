using Microsoft.Reporting.WebForms;
using Pharmacy.DB;
using Pharmacy.Models;
using System;
using System.Collections.Generic;
using System.Data;
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
    public class SaleController : BaseController
    {
        private static List<Stream> m_streams;
        private static int m_currentPageIndex = 0;

        public ActionResult AllSales()
        {
            var saleList = context.Sales.ToList();
            return View(saleList);
        }

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
            var item = context.Items.Where(x => x.ID == itemID).Select(x => new
            {
                ID = x.ID,
                Name = x.Name,
                SalePrice = x.SalePrice,
                SalePercentage = x.SalePercentage,
                TotalStock = x.TotalStock
            }).FirstOrDefault();
            return Json(item, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult SaveSaleItems(List<SaleValidation> sale)
        {
            Sale newSale = new Sale();

            newSale.TotalBeforePercentage = sale.Select(x => x.TotalBeforePercentage).FirstOrDefault();
            newSale.TotalAfterPercentage = sale.Select(x => x.TotalAfterPercentage).FirstOrDefault();
            newSale.AmountPaid = sale.Select(x => x.AmountPaid).FirstOrDefault();
            newSale.Arears = sale.Select(x => x.Arears).FirstOrDefault();

            context.Sales.Add(newSale);
            context.SaveChanges();

            foreach (var n in sale)
            {
                SaleItem saleItem = new SaleItem();

                var itemID = context.Items.Where(x => x.Name == n.Name).Select(x => x.ID).FirstOrDefault();

                saleItem.SaleID = newSale.ID;
                saleItem.ItemID = itemID;
                saleItem.SalePercentage = n.SalePercentage;
                saleItem.Quantity = n.Quantity;

                context.SaleItems.Add(saleItem);
                context.SaveChanges();

                var Item = context.Items.Where(x => x.ID == itemID).FirstOrDefault();

                Item.TotalStock -= n.Quantity;
                context.SaveChanges();
            }

            return Json(true);
        }


        #region Print Code
        public ActionResult Report(List<SaleValidation> sale)
        {
            SaveSaleItems(sale);

            var dt2 = new DataTable();
            dt2.Columns.Add("Name");
            dt2.Columns.Add("Quantity");
            dt2.Columns.Add("SalePrice");
            //dt2.Columns.Add("PerItemTotal");

            //dt2.Columns.Add("TotalAfterPercentage");
            //dt2.Columns.Add("AmountPaid");
            //dt2.Columns.Add("Arears");
            foreach (var item in sale)
            {
                var row = dt2.NewRow();
                row["Name"] = item.Name;
                row["Quantity"] = item.Quantity;
                row["SalePrice"] = item.SalePrice;
                //row["Textbox40"] = 2;
                //row["PerItemTotal"] = item.PerItemTotal;
                //row["PerItemTotal"] = item.PerItemTotal;
                //row["TotalAfterPercentage"] = item.TotalAfterPercentage;
                //row["AmountPaid"] = item.AmountPaid;
                //row["Arears"] = item.Arears;
                dt2.Rows.Add(row);
            }
            
            LocalReport report = new LocalReport();

            report.ReportPath = "Report/SaleReport.rdlc";

            report.DataSources.Add(new ReportDataSource("DataSet1", dt2));

            PrintToPrinter(report);
            return View();
        }

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
    }
}