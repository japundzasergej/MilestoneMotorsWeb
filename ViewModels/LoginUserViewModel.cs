using System.ComponentModel.DataAnnotations;

namespace MilestoneMotorsWeb.ViewModels
{
    public class LoginUserViewModel
    {
        [Required]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
