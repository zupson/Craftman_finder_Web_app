using BL.Dtos;
using System.ComponentModel.DataAnnotations;

namespace WebApp.ViewModels
{
    public class CreateJobApplicationVm
    {
        public int Id { get; set; }
        public int JobPostId { get; set; }
        [Required]
        public  RegisterPersonDto Person { get; set; }
    }

    public class EditJobApplicationVm
    {
        public int Id { get; set; }
        public int JobPostId { get; set; }
        [Required]
        public  EditPersonDto Person { get; set; }
    }
    public class ResponseJobApplicationVm
    {
        //public int Id { get; set; }
        //public int JobPostId { get; set; }
        //public  ResponsePersonDto Person { get; set; }
        public int Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public ResponseJobPostVm? JobPost { get; set; }
        public ResponsePersonDto? Person { get; set; }
    }
}
