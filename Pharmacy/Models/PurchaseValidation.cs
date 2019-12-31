﻿using System;
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


        [Display(Name = "In Stock")]
        public Nullable<int> TotalStock { get; set; }


        public Nullable<int> SupplierID { get; set; }


        public Nullable<System.DateTime> DateOfSupply { get; set; }


        public Nullable<decimal> PurchasePrice { get; set; }


        public Nullable<int> Percentage { get; set; }


        [RegularExpression("([1-9][0-9]*)", ErrorMessage = "Only Natural No")]
        [Required(ErrorMessage = "Provide integer")]
        public int Quantity { get; set; }
    }
}