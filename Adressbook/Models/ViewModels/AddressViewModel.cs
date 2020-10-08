using System.ComponentModel.DataAnnotations;

namespace Adressbook.Models.ViewModels
{
    public class AddressViewModel
    {
        public int Id { get; set; }
        public int ContactId { get; set; }
        [Display(Name = "Adres")]
        public string AddressText { get; set; }
        [Display(Name = "Şehir")]
        public string City { get; set; }
        [Display(Name = "Posta Kodu")]
        [DisplayFormat(DataFormatString = "{0:00}", ApplyFormatInEditMode = true)]
        public decimal ZipCode { get; set; }
        [Display(Name = "Telefon")]
        [DisplayFormat(DataFormatString = "{0:00}", ApplyFormatInEditMode = true)]
        public decimal Phone { get; set; }
    }
}