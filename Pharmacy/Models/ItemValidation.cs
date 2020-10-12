using Pharmacy.DB;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Pharmacy.Models
{
    public class ItemValidation
    {
        public int ID { get; set; }


        [Required(ErrorMessage = "provide BarCode")]
        public string Barcode { get; set; }


        [Required(ErrorMessage = "provide name")]
        public string Name { get; set; }


        public string Description { get; set; }


        public string Shelf { get; set; }


        // [0-9] : for integers which allows integers from 0 to 9
        // d{0,9}: this will take up to 10 digits
        // \. : for decimal
        // d{1,3} : for decimal up to three digits
        [Required(ErrorMessage = "provide purchase price")]
        [RegularExpression(@"^[1-9]\d{0,9}(\.\d{1,2})?%?$", ErrorMessage = "Decimal No with 2 decimal places")]
        public Nullable<decimal> PurchasePrice { get; set; }


        // [0-9] : for integers which allows integers from 0 to 9
        // d{0,9}: this will take up to 10 digits
        // \. : for decimal
        // d{1,3} : for decimal up to three digits
        [Required(ErrorMessage = "provide sale price")]
        [RegularExpression(@"^[1-9]\d{0,9}(\.\d{1,2})?%?$", ErrorMessage = "Decimal No with 2 decimal places")]
        public Nullable<decimal> SalePrice { get; set; }


        [Required(ErrorMessage = "provide pieces per pack")]
        [Display(Name = "Pieces/pack")]
        public Nullable<int> PiecesPerPack { get; set; }


        [Display(Name = "Sale Price/piece")]
        public Nullable<decimal> SalePricePerPiece { get; set; }


        [Display(Name = "Manufacturer")]
        public Nullable<int> ManufacturerID { get; set; }


        [Display(Name = "Item Type")]
        [Required(ErrorMessage = "provide Item Type")]
        public Nullable<int> ItemTypeID { get; set; }


        public ItemTypeValidation ItemTypeValidation { get; set; }

        //Just for the grid to show Item type 
        public string ItemTypeName { get; set; }

        public Nullable<int> PackSize { get; set; }


        [Display(Name = "Purchase %(u get)")]
        [Required(ErrorMessage = "Provide % you'll get")]
        public Nullable<int> PurchasePercentage { get; set; }


        [Display(Name = "Sale % (u give)")]
        [Required(ErrorMessage = "Provide % you'll give")]
        public Nullable<int> SalePercentage { get; set; }


        [Display(Name = "In Stock")]
        public Nullable<int> TotalStock { get; set; }


        [Display(Name = "Other Bonus (u get)")]
        public Nullable<decimal> OtherBonus { get; set; }


        public string GenericName { get; set; }


        [Display(Name = "Strip/Pack")]
        [Required(ErrorMessage = "Provide Strip Per Pack")]
        public int StripPerPack { get; set; }


        //TODO: below line is not working
        [DefaultValue(0)]
        [Display(Name = "Tab/Strip")]
        [Required(ErrorMessage = "Provide Tab Per Strip")]
        public int TabPerStrip { get; set; }

        public Nullable<int> TotalItemPerPack { get; set; }

        public Nullable<int> LooseQuantitySold { get; set; }

        #region Not related to DB just for Specific UI


        [RegularExpression("([1-9][0-9]*)", ErrorMessage = "Only Natural No")]
        [Required(ErrorMessage = "Provide integer")]
        public int Quantity { get; set; }

        public Nullable<int> LowStockQuantity { get; set; }
        public Nullable<int> CriticalLowStockQuantity { get; set; }
        #endregion

    }
}