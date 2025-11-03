using System.ComponentModel.DataAnnotations;

namespace BL.Dtos
{
    public class CreateJobTypeDto
    {
        [Required]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "Job type name must be greater than 2 characters long.")]
        public required string Name { get; set; }
    }

    public class EditJobTypeDto
    {
        [StringLength(100, MinimumLength = 2, ErrorMessage = "Job type name must be greater than 2 characters long.")]
        public string Name { get; set; }
    }

    public class ResponseJobTypeDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = default!;
    }
}
