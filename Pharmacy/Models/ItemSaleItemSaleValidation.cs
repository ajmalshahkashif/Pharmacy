using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Pharmacy.Models
{
    public class ItemSaleItemSaleValidation
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public int Expr1 { get; set; }
        public int Expr2 { get; set; }
        public System.DateTime DateOfSale { get; set; }
        public decimal TotalBeforePercentage { get; set; }
        public decimal TotalAfterPercentage { get; set; }
        public int AmountPaid { get; set; }
        public decimal Arears { get; set; }
        public int SalePercentage { get; set; }
        public int Quantity { get; set; }
        public int ItemID { get; set; }
        public decimal SalePrice { get; set; }
        public int SpecialDiscount { get; set; }
        public string LoosePack { get; set; }
    }
}