using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Adressbook.Models
{
    public class Contact
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public ApplicationUser User { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public bool IsDeleted { get; set; }
        public List<Address> Addresses { get;set; }

    }
}