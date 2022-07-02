namespace EcoFriendsWeb.DataModels
{
    public class Tag
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public Tag()
        {

        }
        public Tag(string name)
        {
            Name = name;
        }
    }
}
