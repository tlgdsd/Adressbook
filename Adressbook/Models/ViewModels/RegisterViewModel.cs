using System.ComponentModel.DataAnnotations;

namespace Adressbook.Models
{
    public class RegisterViewModel
    {
        [Required]
        [Display(Name = "Kullanıcı Adı")]
        public string Username { get; set; }

        [Required]
        [EmailAddress]
        [Display(Name = "E-mail Adresi")]
        public string Email { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Parola")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Parola Tekrar")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
    }
}
