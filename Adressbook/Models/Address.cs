using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Adressbook.Models
{
    public class Address
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public Contact Contact { get; set; }
        public string AddressText { get; set; }
        public string City { get; set; }
        public decimal ZipCode { get; set; }
        public decimal Phone { get; set; }
    }
}