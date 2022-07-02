using EcoFriendsWeb.DataModels;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EcoFriendsWeb.ViewModels
{
    public class CreateEventViewModel
    {
        public List<ImagePicture> ImagesStore { get; set; }
        public List<Tag> Tags { get; set; }
        [Required(AllowEmptyStrings =false)]
        public string Title { get; set; }
        [Required(AllowEmptyStrings = false)]
        public string ShortDescription { get; set; }
        [Required(AllowEmptyStrings = false)]
        public string Desctiption { get; set; }
        [Required]
        public string EventData { get; set; }
        [Required(AllowEmptyStrings = false)]
        public string Location { get; set; }
        public string Text { get; set; }
        public string Text2 { get; set; }
        public string TimeCreate { get; set; }
        [DataType(DataType.PhoneNumber)]
        public string ContactPhone { get; set; }
        [DataType(DataType.EmailAddress)]
        public string ContactEmail { get; set; }
        public string ContactAdress { get; set; }
    }
}
