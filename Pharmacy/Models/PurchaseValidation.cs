using Pharmacy.DB;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Pharmacy.Models
{
    public class PurchaseValidation
    {
        public int ID { get; set; }


        [Required(ErrorMessage = "provide name")]
        public string Name { get; set; }


        [Display(Name = "Purchase %(u get)")]
        [Required(ErrorMessage = "Provide % you'll get")]
        public Nullable<int> PurchasePercentage { get; set; }

        [Display(Name = "Purchase %(u give)")]
        [Required(ErrorMessage = "Provide % you'll give")]
        public Nullable<int> SalePercentage { get; set; }


        [RegularExpression("([1-9][0-9]*)", ErrorMessage = "Only Natural No")]
        [Required(ErrorMessage = "Provide integer")]
        public Nullable<decimal> PurchasePrice { get; set; }


        [Display(Name = "In Stock")]
        public int TotalStock { get; set; }

        public Nullable<System.DateTime> DateOfSupply { get; set; }

        public Nullable<int> Percentage { get; set; }


        [RegularExpression("([1-9][0-9]*)", ErrorMessage = "Only Natural No")]
        [Required(ErrorMessage = "Provide integer")]
        public int Quantity { get; set; }

        public int NetTotal { get; set; }


        [RegularExpression("([1-9][0-9]*)", ErrorMessage = "Only No")]
        [Required(ErrorMessage = "Provide integer")]
        public Nullable<decimal> AmountPaid { get; set; }


        public Nullable<decimal> Arears { get; set; }


        public Nullable<int> SupplierID { get; set; }
        public virtual Supplier Supplier { get; set; }

    }
}