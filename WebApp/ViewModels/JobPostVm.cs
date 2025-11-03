using BL.Dtos;
using System.ComponentModel.DataAnnotations;

namespace WebApp.ViewModels
{
    public class CreateJobPostVm
    {
        public int Id { get; set; }
        [Required]
        public  CreateContractorDto Contractor { get; set; }
        [Required]
        public CreateLocationDto Location { get; set; }
    }
    public class EditJobPostVm
    {
        public int Id { get; set; }
        public EditContractorDto Contractor { get; set; }
        public EditLocationDto Location { get; set; }
    }
    public class ResponseJobPostVm
    {
        public int Id { get; set; }
        public ResponseContractorLocationDto ContractorLocation { get; set; } = default!;

        //public ResponseContractorDto Contractor { get; set; }
        //public ResponseLocationDto Location { get; set; }
    }
}