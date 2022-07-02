using EcoFriendsWeb.DataModels;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EcoFriendsWeb.ViewModels
{
    public class UserViewModels
    {
        public User User { get; set; }

        [Display(Name ="Текущий пароль")]
        public string NowPassword { get; set; }

        [Display(Name = "новый пароль")]
        public string NewPassword { get; set; }
    }
}
