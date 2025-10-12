using Shared.Validators;
using System.ComponentModel.DataAnnotations;

namespace MVC.Areas.Identity.ViewModels
{
    public class RegisterViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; } = "";

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; } = "";

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; } = "";

        [Required]
        [StringLength(255, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 1)]
        [Display(Name = "First Name")]
        public string FirstName { get; set; } = "";
        
        [Required]
        [StringLength(255, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 1)]
        [Display(Name = "Last Name")]
        public string LastName { get; set; } = "";

        [Required]
        [Display(Name = "Sex")]
        public char Sex { get; set; }

        [Required]
        [Display(Name = "Date of Birth")]
        [MinimumAge(18, ErrorMessage = "The user must be at least 18 years old")]
        public DateTime DateOfBirth { get; set; } = DateTime.Now.AddYears(-18);

        // the city is fixed for now
        [Required]
        public int CityId { get; set; } = 1270426;
    }
}
