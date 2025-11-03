using System.ComponentModel.DataAnnotations;

namespace BL.Dtos
{

    public class CreateLocationDto 
    {
        public int? Id { get; set; } //prilikom kreiranja sam se psotavlja 

        [Required]
        [StringLength(7, ErrorMessage = "PostalCode cannot be longer than 7 characters.")]
        public required string PostalCode { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "Town name should be between 3 and 100 characters long!")]
        public required string TownName { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "Country name should be between 3 and 100 characters long!")]
        public required string CountryName { get; set; }
    }

    public class EditLocationDto 
    {
        public int? Id { get; set; } //prilikom kreiranja sam se psotavlja 

        [StringLength(7, ErrorMessage = "PostalCode cannot be longer than 7 characters.")]
        public string PostalCode { get; set; }

        [StringLength(100, MinimumLength = 3, ErrorMessage = "Town name should be between 3 and 100 characters long!")]
        public string TownName { get; set; }

        [StringLength(100, MinimumLength = 3, ErrorMessage = "Country name should be between 3 and 100 characters long!")]
        public string CountryName { get; set; }
    }

    public class ResponseLocationDto
    {
        public int Id { get; set; }
        public string PostalCode { get; set; } = default!;
        public int TownId { get; set; }
        public string TownName { get; set; } = default!;
        public string CountryName { get; set; } = default!;
    }
}
