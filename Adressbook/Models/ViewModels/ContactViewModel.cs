using System.ComponentModel.DataAnnotations;

namespace Adressbook.Models.ViewModels
{
    public class ContactViewModel
    {
        public int Id { get; set; }
        [Required]
        [Display(Name ="İsim")]
        public string Name { get; set; }
        [Required]
        [Display(Name = "Soyisim")]
        public string Surname { get; set; }
        [Display(Name = "Adres")]
        public string AddressText { get; set; }
        [Display(Name = "Şehir")]
        public string AddressCity { get; set; }
        [Display(Name = "Posta Kodu")]
        public decimal AddressZipCode { get; set; }
        [Display(Name = "Telefon")]
        public decimal AddressPhone { get; set; }
    }
}