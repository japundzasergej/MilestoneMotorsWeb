using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace MilestoneMotorsWeb.Models
{
    public class User : IdentityUser
    {
        public string? ProfilePictureImageUrl { get; set; }
        public string? Country { get; set; }
        public string? City { get; set; }
        public string? State { get; set; }
        public ICollection<Car> MyListings { get; set; }
    }
}
