using System.ComponentModel.DataAnnotations;

namespace Adressbook.Models
{
    public class LoginViewModel
    {
        [Required]
        [Display(Name = "Kullanıcı Adı")]
        [EmailAddress]
        public string Username { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Şifre")]
        public string Password { get; set; }

        [Display(Name = "Beni Hatırla")]
        public bool RememberMe { get; set; }
    }
}
