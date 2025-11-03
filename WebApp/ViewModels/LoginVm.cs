using System.ComponentModel.DataAnnotations;

namespace WebApp.ViewModels
{
    public class LoginVm
    {
        [Required(ErrorMessage = "Username is required")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "Username should be at least 3 characters long!")]
        public string Username { get; set; } 

        [Required(ErrorMessage = "Password is required")]
        [StringLength(50, MinimumLength = 8, ErrorMessage = "Password should be should be at least 8 characters long!")]
        public string Password { get; set; }

        public string? ReturnUrl { get; set; }
    }
}
