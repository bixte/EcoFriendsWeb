using EcoFriendsWeb.DataModels;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EcoFriendsWeb.ViewModels
{
    public class CreatePostViewModel
    {
        [Display(Name ="Заголовок")]
        public string Title { get; set; }

        [Display(Name = "Короткое описание")]
        public string ShortDesctiption { get; set; }

        [Display(Name = "Полное описание")]
        public string Desctiption { get; set; }

        [Display(Name ="Теги")]
        public List<Tag> Tags { get; set; }

        public List<PostPartical> PostParticals { get; set; }
    }
}
