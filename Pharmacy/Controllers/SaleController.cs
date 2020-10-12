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
using CrystalDecisions.Shared;
using CrystalDecisions.CrystalReports.Engine;
using Pharmacy.Report;
using System.Windows.Forms;
using Kendo.Mvc.UI;
using Kendo.Mvc.Extensions;

namespace Pharmacy.Controllers
{
    public class SaleController : BaseController
    {

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
            var AllItems = context.Items.Select(x => new { ID = x.ID, Name = x.Name }).ToList();
            var ItemList = AllItems.Where(x => x.Name.StartsWith(Prefix, StringComparison.OrdinalIgnoreCase)).Select(x => new { Name = x.Name, ID = x.ID }).ToList();
            //var ItemList = AllItems.Where(x => x.Name.StartsWith(Prefix, StringComparison.OrdinalIgnoreCase)).Select(x => new { Name = x.Name, ID = x.ID }).ToList();

            //var ItemList = context.Items.Where(x => x.Name.Contains(Prefix)).Select(x => new { Name = x.Name, ID = x.ID }).ToList();

            return Json(ItemList, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetItemByID(int? itemID)
        {
            var item = context.Items.Where(x => x.ID == itemID).Select(x => new
            {
                ID = x.ID,
                Name = x.Name,
                SalePrice = x.SalePrice,
                SalePercentage = x.SalePercentage,
                TotalStock = x.TotalStock,
                ItemTypeID = x.ItemTypeID,
                PiecesPerPack = x.PiecesPerPack
            }).FirstOrDefault();
            return Json(item, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetItemByBarCode(string barCode)
        {
            var item = context.Items.Where(x => x.Barcode == barCode).Select(x => new
            {
                ID = x.ID,
                Barcode = x.Barcode,
                Name = x.Name,
                SalePrice = x.SalePrice,
                SalePercentage = x.SalePercentage,
                TotalStock = x.TotalStock,
                ItemTypeID = x.ItemTypeID,
                PiecesPerPack = x.PiecesPerPack
            }).FirstOrDefault();
            return Json(item, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public JsonResult SaveSaleItems(List<SaleValidation> sale)
        {
            Sale newSale = new Sale();

            newSale.TotalBeforePercentage = sale.Select(x => x.TotalBeforePercentage).FirstOrDefault();

            decimal? totalAfterPercentage = sale.Select(x => x.TotalAfterPercentage).FirstOrDefault();
            //if(totalAfterPercentage > )
            newSale.TotalAfterPercentage = sale.Select(x => x.TotalAfterPercentage).FirstOrDefault();
            newSale.SpecialDiscount = sale.Select(x => x.SpecialDiscount).FirstOrDefault();
            newSale.AmountPaid = sale.Select(x => x.AmountPaid).FirstOrDefault();
            newSale.Arears = sale.Select(x => x.Arears).FirstOrDefault();
            newSale.DateOfSale = DateTime.Now;

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
                saleItem.LoosePack = n.LoosePack;

                context.SaleItems.Add(saleItem);
                context.SaveChanges();

                var Item = context.Items.Where(x => x.ID == itemID).FirstOrDefault();

                if (n.LoosePack == "P")
                    Item.TotalStock -= n.Quantity;
                else if (n.LoosePack == "L")
                {
                    Item.LooseQuantitySold += n.Quantity;
                    if (Item.LooseQuantitySold >= n.PiecesPerPack)
                    {
                        int? packSold = Item.LooseQuantitySold / n.PiecesPerPack;
                        int? looseQuantityleftAfterPackomission = Item.LooseQuantitySold % n.PiecesPerPack;
                        Item.TotalStock -= packSold;
                        Item.LooseQuantitySold = looseQuantityleftAfterPackomission;

                    }
                }
                //TODO: remove last line. just to save saleprice at run time
                Item.SalePrice = n.SalePrice;
                context.SaveChanges();
            }

            return Json(true);
        }

        public JsonResult GetSaleList([DataSourceRequest]DataSourceRequest request)
        {
            var saleList = context.Sales.Select(x => new
            {
                ID = x.ID,
                SpecialDiscount = x.SpecialDiscount,
                TotalAfterPercentage = x.TotalAfterPercentage,
                TotalBeforePercentage = x.TotalBeforePercentage,
                AmountPaid = x.AmountPaid,
                DateOfSale = x.DateOfSale,
                Arears = x.Arears
            }).ToList();
            return this.Json(saleList.ToDataSourceResult(request));
        }

        [HttpPost]
        public JsonResult GetItemTypeName(int? ItemTypeID)
        {
            var item = context.ItemTypes.Where(x => x.ID == ItemTypeID).Select(x => new
            {
                ID = x.ID,
                Name = x.Name,
                isActive = x.isActive

            }).FirstOrDefault();
            return Json(item, JsonRequestBehavior.AllowGet);
        }

        #region Print Code

        public ActionResult Report(List<ItemSaleItemSaleValidation> sale)
        {
            List<SaleValidation> saleValidation = new List<SaleValidation>();
            foreach (var s in sale) /*SpecialDiscount = 0*/
            {
                SaleValidation sv = new SaleValidation();

                sv.Name = s.Name;
                sv.Quantity = s.Quantity;
                sv.SalePrice = s.SalePrice;
                sv.TotalBeforePercentage = s.TotalBeforePercentage;
                sv.LoosePack = s.LoosePack;
                sv.TotalAfterPercentage = s.TotalAfterPercentage;
                sv.SpecialDiscount = s.SpecialDiscount;
                sv.AmountPaid = s.AmountPaid;
                sv.Arears = s.Arears;
                saleValidation.Add(sv);
            }
            SaveSaleItems(saleValidation);

            PrintDialog printDialog = new PrintDialog();
            //if (printDialog.ShowDialog() == DialogResult.OK)
            {
                ReportDocument reportDocument = new ReportDocument();
                //reportDocument.Load(Application.StartupPath + "\\CustomerList.rpt");
                reportDocument.Load(Path.Combine(Server.MapPath("~/Report"), "CustomerList.rpt"));
                reportDocument.SetDataSource(sale);
                reportDocument.PrintOptions.PrinterName = printDialog.PrinterSettings.PrinterName;

                //reportDocument.PrintToPrinter(printDialog.PrinterSettings.Copies, printDialog.PrinterSettings.Collate,
                //    printDialog.PrinterSettings.FromPage, printDialog.PrinterSettings.ToPage);

                reportDocument.PrintToPrinter(1, true, 0, 0);

            }

            return Content("hello");

        }

        public ActionResult Report2(List<ItemSaleItemSale> sale)
        {
            //SaveSaleItems(sale);

            ReportDocument rd = new ReportDocument();
            rd.Load(Path.Combine(Server.MapPath("~/Report"), "CustomerList.rpt"));
            rd.SetDataSource(sale);

            var stream = rd.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
            var pdfbytearray = new byte[stream.Length - 1];

            stream.Position = 0;
            stream.Read(pdfbytearray, 0, Convert.ToInt32(stream.Length - 1));
            Response.ClearContent();
            Response.ClearHeaders();
            Response.AddHeader("content-disposition", "filename=Test.pdf");

            Response.ContentType = "application/pdf";
            Response.AddHeader("content-length", pdfbytearray.Length.ToString());
            Response.BinaryWrite(pdfbytearray);

            PrintDocument pd = new PrintDocument();

            pd.Print();


            return Content("hello");

        }

        public ActionResult Report3(List<ItemSaleItemSale> sale)
        {
            CustomerList rpt = new CustomerList();
            rpt.SetDataSource(sale);

            Stream s = rpt.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);

            return File(s, "application/pdf");


        }

        #endregion
    }
}
