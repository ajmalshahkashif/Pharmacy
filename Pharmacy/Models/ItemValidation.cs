using Pharmacy.DB;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Pharmacy.Models
{
    public class ItemValidation
    {
        public int ID { get; set; }

        [Required(ErrorMessage = "Please provide name")]
        public string Name { get; set; }

        public string Description { get; set; }
        public string Shelf { get; set; }

        [Required(ErrorMessage = "Please provide purchase price")]
        public Nullable<double> PurchasePrice { get; set; }


        [Required(ErrorMessage = "Please provide sale price")]
        public Nullable<double> SalePrice { get; set; }


        [Required(ErrorMessage = "Please provide pieces per pack")]
        [Display(Name = "Pieces/pack")]
        public Nullable<int> PiecesPerPack { get; set; }


        [Display(Name = "Sale Price/piece")]
        public Nullable<decimal> SalePricePerPiece { get; set; }

        public Nullable<decimal> Discount { get; set; }


        [Display(Name = "Manufacturer")]
        [Required(ErrorMessage = "Please provide Company name")]
        public Nullable<int> ManufacturerID { get; set; }
        

        [Display(Name= "Item Type")]
        [Required(ErrorMessage = "Please provide Item Type")]
        public Nullable<int> ItemTypeID { get; set; }

    }
}