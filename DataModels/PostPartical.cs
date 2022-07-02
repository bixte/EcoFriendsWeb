using System.Collections.Generic;

namespace EcoFriendsWeb.DataModels
{
    public class PostPartical
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public List<ImagePicture> ImagesStore{ get; set; }
        public List<SubText> SubTexts { get; set; }
    }
}
