namespace EcoFriendsWeb.DataModels
{
    public class ImagePicture
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Path { get; set; }
        public ImagePicture() {}
        public ImagePicture(string name, string path)
        {
            Name = name;
            Path = path;
        }
    }
}
