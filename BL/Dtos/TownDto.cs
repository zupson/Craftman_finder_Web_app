using System.ComponentModel.DataAnnotations;

namespace BL.Dtos
{
    public class CreateTownDto
    {
        [Required]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "Name should be between 3 and 100 characters long!")]
        public required string Name { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "Country name should be between 3 and 100 characters long!")]
        public required string CountryName { get; set; }
    }

    public class EditTownDto
    {
        [StringLength(100, MinimumLength = 3, ErrorMessage = "Name should be between 3 and 100 characters long!")]
        public string Name { get; set; }

        [StringLength(100, MinimumLength = 3, ErrorMessage = "Country name should be between 3 and 100 characters long!")]
        public string CountryName { get; set; }
    }

    public class ResponseTownDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = default!;
        public string CountryName { get; set; } = default!;
    }
}
