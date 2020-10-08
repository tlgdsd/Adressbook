using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Adressbook.Models
{
    public class Contact
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public ApplicationUser User { get; set; }
        [Display(Name = "Adı")]
        public string Name { get; set; }
        [Display(Name = "Soyadı")]
        public string Surname { get; set; }
        public bool IsDeleted { get; set; }
        public List<Address> Addresses { get;set; }

    }
}