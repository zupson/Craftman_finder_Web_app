namespace BL.Dtos
{
    public class CreateJobApplicationDto
    {
        public int JobPostId { get; set; }
        public required RegisterPersonDto Person { get; set; }
    }

    public class EditJobApplicationDto 
    {
        public int JobPostId { get; set; }
        public  EditPersonDto Person { get; set; }
    }

    public class ResponseJobApplicationDto
    {
        public int Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public ResponseJobPostDto? JobPost { get; set; }
        public ResponsePersonDto? Person { get; set; }
    }
}
