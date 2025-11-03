using System.ComponentModel.DataAnnotations;

namespace BL.Dtos
{

    public class CreateCountryDto
    {
        [Required]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "Country name should be between 3 and 100 characters long!")]
        public required string Name { get; set; }
    }
    public class EditCountryDto
    {
        [StringLength(100, MinimumLength = 3, ErrorMessage = "Country name should be between 3 and 100 characters long!")]
        public string Name { get; set; }
    }

    public class ResponseCountryDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = default!;
    }

}
