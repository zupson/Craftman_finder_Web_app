using System.ComponentModel.DataAnnotations;

namespace BL.Dtos
{
   
    public class CreateContractorDto 
    {
        public int PersonId { get; set; } // može biti null ako nije freelancer
        [StringLength(50, MinimumLength = 2, ErrorMessage = "Compny name must be greater than 2 characters long.")]
        public string? CompanyName { get; set; }
        [Required]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "Job type name name must be greater than 2 characters long.")]
        public required string JobTypeName { get; set; }
        public bool? IsFreelancer { get; set; }
        public RegisterPersonDto Person { get; set; }
        public IList<CreateLocationDto> Locations { get; set; } = new List<CreateLocationDto>();
    }

    public class EditContractorDto
    {
        public int Id { get; set; }
        public int PersonId { get; set; } // može biti null ako nije freelancer

        [StringLength(50, MinimumLength = 2, ErrorMessage = "Compny name must be greater than 2 characters long.")]
        public string? CompanyName { get; set; }

        [StringLength(50, MinimumLength = 2, ErrorMessage = "Job type name name must be greater than 2 characters long.")]
        public string JobTypeName { get; set; }
        public bool? IsFreelancer { get; set; }
        public EditPersonDto Person { get; set; }
        public IList<EditLocationDto> Locations { get; set; } = new List<EditLocationDto>();
    }

    public class ResponseContractorDto
    {
        public int Id { get; set; }
        public string? CompanyName { get; set; } //moze biti null ako je contructor = freelancer
        public int JobTypeId { get; set; }
        public string JobTypeName { get; set; } = default!;
        public bool? IsFreelancer { get; set; }
        public ResponsePersonDto Person { get; set; }
        public IList<ResponseLocationDto> Locations { get; set; } = new List<ResponseLocationDto>();
    }
}
