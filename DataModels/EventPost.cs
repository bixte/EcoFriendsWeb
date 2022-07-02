using EcoFriendsWeb.DataModels;
using System.Collections.Generic;

namespace EcoFriendsWeb.DataModels
{
    public class EventPost
    {
        public int Id { get; set; }
        public List<ImagePicture> ImagesStore { get; set; }
        public List<Tag> Tags { get; set; }
        public string Title { get; set; }
        public string ShortDescription { get; set; }
        public string Desctiption { get; set; }
        public string EventData { get; set; }
        public string Location { get; set; }
        public string Text { get; set; }
        public string Text2 { get; set; }
        public string TimeCreate { get; set; }
        public string ContactPhone { get; set; }
        public string ContactEmail { get; set; }
        public string ContactAdress { get; set; }
        public List<User> Users { get; set; }
        public List<Comment> Comments { get; set; }


    }
}
