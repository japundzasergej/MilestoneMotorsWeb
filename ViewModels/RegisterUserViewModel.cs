using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MilestoneMotorsWeb.ViewModels
{
    public class RegisterUserViewModel
    {
        [Required]
        public string Username { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [StringLength(
            100,
            MinimumLength = 8,
            ErrorMessage = "Password must be at least 8 characters long."
        )]
        [RegularExpression(
            @"^(?=.*[A-Z])(?=.*[0-9])(?=.*[!@#$%^&*()\-__+.]).{8,}$",
            ErrorMessage = "Password must have at least one uppercase letter, one digit, one special character."
        )]
        public string Password { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [DisplayName("Confirm Password")]
        [Compare("Password", ErrorMessage = "Password must match.")]
        public string ConfirmPassword { get; set; }
    }
}
