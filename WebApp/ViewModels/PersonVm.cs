using System.ComponentModel.DataAnnotations;

namespace WebApp.ViewModels
{
    public class PersonVm
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "First name is required")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "First should be at least 3 characters long!")]
        public  string FirstName { get; set; }

        [Required(ErrorMessage = "Last name is required")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "Last should be at least 3  characters long!")]
        public  string LastName { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email format!")]
        [StringLength(200)]
        public  string Email { get; set; }

        [Required(ErrorMessage = "Phone is required")]
        [Phone(ErrorMessage = "Provide a correct phone number")]
        public  string Phone { get; set; }

        [Required(ErrorMessage = "Username is required")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "Username should be at least 3 characters long!")]
        public  string Username { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [StringLength(50, MinimumLength = 8, ErrorMessage = "Password should be at least 8 characters long!")]
        public  string Password { get; set; }
    }

    public class EditPersonVm
    {
        public int Id { get; set; }
        public  string? FirstName { get; set; }
        public  string? LastName { get; set; }

        [EmailAddress]
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public string? Username { get; set; }
        public string? Password { get; set; }
    }
}
