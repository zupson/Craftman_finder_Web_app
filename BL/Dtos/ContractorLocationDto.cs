namespace BL.Dtos
{
    public class CreateContractorLocationDto
    {
        public int ContractorId { get; set; }
        public int LocationId { get; set; }
    }
    public class EditContractorLocationDto : CreateContractorLocationDto
    {
    }
    public class ResponseContractorLocationDto
    {
        public int Id { get; set; }
        public int ContractorId { get; set; }
        public int LocationId { get; set; }
    }
}
