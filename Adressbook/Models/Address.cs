using System.ComponentModel.DataAnnotations;

namespace Adressbook.Models
{
    public class Address
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public Contact Contact { get; set; }
        public bool IsDeleted { get; set; }
        [Display(Name = "Adres")]
        public string AddressText { get; set; }
        [Display(Name = "Şehir")]
        public string City { get; set; }
        [DisplayFormat(DataFormatString = "{0:00}", ApplyFormatInEditMode = true)]
        [Display(Name = "Posta Kodu")]
        public decimal ZipCode { get; set; }
        [DisplayFormat(DataFormatString = "{0:00}", ApplyFormatInEditMode = true)]
        [Display(Name = "Telefon")]
        public decimal Phone { get; set; }
    }
}