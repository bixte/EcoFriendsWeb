using System.ComponentModel.DataAnnotations;

namespace EcoFriendsWeb.ViewModels
{
    public class RegistrationViewModel
    {
        [Required]
        [MinLength(5, ErrorMessage ="Псевдоним должен содержать более 5 символов")]
        public string NickName { get; set; }
        [Required]
        [MinLength(6,ErrorMessage ="Пароль должен содержать более 6 символов")]
        [DataType(DataType.Password)]
        [Display(Name = "Пароль")]
        public string Password { get; set; }

        [Required]
        [Compare("Password", ErrorMessage ="Не совпадает с паролем")]
        [Display(Name = "Подтверждение пароля")]
        public string ConfirmPassword { get; set; }

        [Required]
        [Display(Name ="Телефон")]
        [DataType(DataType.PhoneNumber)]
        public string Phone { get; set; }

        [Required]
        [Display(Name ="Email адрес")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
    }
}
