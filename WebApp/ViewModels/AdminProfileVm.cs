using System.ComponentModel.DataAnnotations;

namespace WebApp.ViewModels
{
    public class AdminProfileVm
    {
        [Required(ErrorMessage = "First name is required")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "First should be at least 3 characters long!")]
        public required string FirstName { get; set; }

        [Required(ErrorMessage = "Last name is required")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "Last should be at least 3  characters long!")]
        public required string LastName { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email format!")]
        [StringLength(200)]
        public required string Email { get; set; }

        [Required(ErrorMessage = "Phone is required")]
        [Phone(ErrorMessage = "Provide a correct phone number")]
        public required string Phone { get; set; }

        [Required(ErrorMessage = "Username is required")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "Username should be at least 3 characters long!")]
        public required string Username { get; set; }
    }
}
