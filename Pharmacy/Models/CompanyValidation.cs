using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Pharmacy.Models
{
    public class CompanyValidation
    {
        public int ID { get; set; }

        [Required(ErrorMessage = "Please name")]
        public string Name { get; set; }
        public byte[] Logo { get; set; }
        public string Address { get; set; }
        public string Description { get; set; }
        public Nullable<bool> isActive { get; set; }
    }
}