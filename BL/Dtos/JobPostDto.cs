namespace BL.Dtos
{
    public class CreateJobPostDto
    {
        public required CreateContractorDto Contractor { get; set; }
        public required CreateLocationDto Location { get; set; } 
    }
    public class EditJobPostDto
    {
        public EditContractorDto Contractor { get; set; }
        public EditLocationDto Location { get; set; } 
    }

    public class ResponseJobPostDto
    {
        public int Id { get; set; }
        public bool IsDeleted { get; set; }
        public ResponseContractorLocationDto ContractorLocation { get; set; } = default!;
    }
}
