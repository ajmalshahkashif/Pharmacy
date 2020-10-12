using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Pharmacy.Models
{
    public class SaleReturnValidation
    {
        //[Display(Name = "Purchase %(u get)")]
        //[Required(ErrorMessage = "Provide % you'll get")]
        //[RegularExpression("([1-9][0-9]*)", ErrorMessage = "Only Natural No")]
        public int ID { get; set; }

        [Required(ErrorMessage = "provide name")]
        public string Name { get; set; }

        public Nullable<int> CustomerID { get; set; }
        
        public Nullable<System.DateTime> DateOfPurchase { get; set; }
        
        public Nullable<decimal> TotalBeforePercentage { get; set; }

        public Nullable<decimal> TotalAfterPercentage { get; set; }

        public Nullable<int> SpecialDiscount { get; set; }

        public Nullable<int> AmountPaid { get; set; }
        
        public Nullable<decimal> Arears { get; set; }
        
        public Nullable<int> ItemID { get; set; }
        
        public Nullable<int> SalePercentage { get; set; }
        
        public Nullable<int> Quantity { get; set; }

        [Display(Name = "In Stock")]
        public int TotalStock { get; set; }
        
        public Nullable<decimal> SalePrice { get; set; }

        public Nullable<decimal> PerItemTotal { get; set; }

        public Nullable<int> TotalItemPerPack { get; set; }

        public string LoosePack { get; set; }

        public Nullable<int> PiecesPerPack { get; set; }
    }
}