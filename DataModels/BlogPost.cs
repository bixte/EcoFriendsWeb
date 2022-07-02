using System;
using System.Collections.Generic;

namespace EcoFriendsWeb.DataModels
{
    public class BlogPost
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string ShortDesctiption { get; set; }
        public string Desctiption { get; set; }
        public string TimeCreate { get; set; }
        public List<Tag> Tags { get; set; }
        public List<ImagePicture> MainImagesStore { get; set; }
        public List<PostPartical> PostParticals { get; set; }
        public List<Comment> Comments { get; set; }
    }
}
