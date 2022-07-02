using EcoFriendsWeb.DataModels;
using System.Linq;

namespace EcoFriendsWeb.ViewModels
{
    public class EventsViewModel
    {
        public IQueryable<EventPost> EventPosts { get; set; }
    }
}
