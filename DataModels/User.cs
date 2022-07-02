using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EcoFriendsWeb.DataModels
{
    public class User : IdentityUser
    {
        public User(string userName) : base(userName)
        {   
        }
        public User()
        {
        }

        public string AvatarPath { get; set; }
        [Display(Name ="О себе")]
        public string Description { get; set; }

        [Display(Name ="Город")]
        public string City { get; set; }

        [Display(Name ="Адрес")]
        public string Adress { get; set; }
        public string Coins { get; set; }
    }
}
