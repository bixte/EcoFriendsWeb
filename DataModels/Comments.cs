using EcoFriendsWeb.DataModels;

namespace EcoFriendsWeb.DataModels
{
    public class Comment
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Text { get; set; }
        public string DataCreate { get; set; }
        public User User { get; set; }
    }
}
