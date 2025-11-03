using System.ComponentModel.DataAnnotations;

namespace WebApp.ViewModels
{

    public class CreateLocationVm
    {
        public int Id { get; set; } 

        [Required]
        [StringLength(7, ErrorMessage = "PostalCode cannot be longer than 7 characters.")]
        public string PostalCode { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "Town name should be between 3 and 100 characters long!")]
        public string TownName { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "Country name should be between 3 and 100 characters long!")]
        public string CountryName { get; set; }
    }
    public class EditLocationVm
    {
        [Required]
        [StringLength(7, ErrorMessage = "PostalCode cannot be longer than 7 characters.")]
        public string PostalCode { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "Town name should be between 3 and 100 characters long!")]
        public string TownName { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "Country name should be between 3 and 100 characters long!")]
        public string CountryName { get; set; }
    }
    public class ResponseLocationVm
    {
        public int Id { get; set; }

        [Display(Name = "Postal code")]
        public string PostalCode { get; set; } = default!;
        public int TownId { get; set; }

        [Display(Name = "Town name")]
        public string TownName { get; set; } = default!;

        [Display(Name = "Country name")]
        public string CountryName { get; set; } = default!;
    }
}
