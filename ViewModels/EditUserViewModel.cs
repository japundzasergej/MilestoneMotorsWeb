using System.ComponentModel;

namespace MilestoneMotorsWeb.ViewModels
{
    public class EditUserViewModel
    {
        [DisplayName("Change Profile Picture (optional)")]
        public IFormFile? ProfilePictureImageUrl { get; set; }
        public string? Country { get; set; }
        public string? City { get; set; }
        public string? State { get; set; }
    }
}
