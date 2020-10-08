using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Adressbook.Models.ViewModels
{
    public class AddressViewModel
    {
        public int ContactId { get; set; }
        public string AddressText { get; set; }
        public string City { get; set; }
        public decimal ZipCode { get; set; }
        public decimal Phone { get; set; }
    }
}