using MilestoneMotorsWeb.Models;

namespace MilestoneMotorsWeb.ViewModels
{
    public class UserAccountViewModel
    {
        public string? ProfilePictureImageUrl { get; set; }
        public string? Country { get; set; }
        public string? City { get; set; }
        public string? State { get; set; }
        public ICollection<Car> MyListings { get; set; }
    }
}
