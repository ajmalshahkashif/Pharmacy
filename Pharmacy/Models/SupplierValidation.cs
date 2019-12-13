using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Pharmacy.Models
{
    public class SupplierValidation
    {
        public int ID { get; set; }

        [Required(ErrorMessage = "Please type name")]
        public string Name { get; set; }


        [Display(Name = "Active")]
        public bool isActive { get; set; }

        public string Description { get; set; }

    }
}