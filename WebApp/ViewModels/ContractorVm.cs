using BL.Dtos;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace WebApp.ViewModels
{
    public class CreateContractorVm
    {
        public int Id { get; set; }
        public string? CompanyName { get; set; }
        [Required]
        public string JobTypeName { get; set; } = string.Empty;
        public bool IsFreelancer { get; set; }

        [Required]
        public PersonVm Person { get; set; } = new PersonVm();
        //sve lok i baze
        public List<SelectListItem> AllLocations { get; set; } = new List<SelectListItem>();
        //id seletiranik lokacija 
        public List<int> SelectedLocationIds { get; set; } = new List<int>();
        //opcija dodavanja nove lokacije
        public ResponseLocationVm? NewLocation { get; set; } 

    }
    public class EditContractorVm
    {
        public int Id { get; set; }
        public EditPersonVm Person { get; set; }
        public string? CompanyName { get; set; }
        public string? JobTypeName { get; set; }
        public bool IsFreelancer { get; set; }
        //sve lok i baze
        public List<SelectListItem>? AllLocations { get; set; } = new List<SelectListItem>();
        //id seletiranik lokacija 
        public List<int>? SelectedLocationIds { get; set; } = new List<int>();
        //opcija dodavanja nove lokacije
        public ResponseLocationVm? NewLocation { get; set; } = new ResponseLocationVm();
    }

    public class ResponseContractorVm
    {
        public int Id { get; set; }
        [Display(Name = "Company name")]
        public string? CompanyName { get; set; }
        [Display(Name = "Job type")]
        public string JobTypeName { get; set; }
        public bool IsFreelancer { get; set; }
        public ResponsePersonDto Person { get; set; }
        public List<ResponseLocationVm> Locations { get; set; } = new List<ResponseLocationVm>();
    }
}
