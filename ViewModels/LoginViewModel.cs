using System.ComponentModel.DataAnnotations;

namespace EcoFriendsWeb.ViewModels
{
    public class LoginViewModel
    {

        [Required]
        [MinLength(5)]
        [Display(Name = "Ваш псевдоним")]
        public string NickName { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Пароль")]
        public string Password { get; set; }

        public bool RememberMe { get; set; }

        #nullable enable
        public string? ReturnUrl { get; set; }

    }
}
