using AutoMapper;
using BL.Dtos;
using WebApp.ViewModels;

namespace WebApp.AutoMapper
{
    public class DtoToVmMappingProfile : Profile
    {
        public DtoToVmMappingProfile()
        {
            CreateMap<PersonVm, RegisterPersonDto>();
            CreateMap<RegisterPersonDto, PersonVm>();
            CreateMap<CreateLocationDto, CreateLocationVm>();
            CreateMap<CreateLocationVm, CreateLocationDto>();
            CreateMap<EditPersonVm, RegisterPersonDto>();
            CreateMap<ResponseLocationDto, ResponseLocationVm>();
            CreateMap<ResponsePersonDto, EditPersonVm>();
            CreateMap<ResponseLocationDto, ResponseLocationVm>();

            CreateMap<ResponseContractorDto, ResponseContractorVm>()
                .ForMember(dest => dest.Id,
                    opt => opt.MapFrom(src => src.Id))
                .ForPath(dest => dest.Person.Id,
                    opt => opt.MapFrom(src => src.Person != null ? src.Person.Id : 0))
                .ForPath(dest => dest.Person.FirstName,
                    opt => opt.MapFrom(src => src.Person != null ? src.Person.FirstName : null))
                .ForPath(dest => dest.Person.LastName,
                    opt => opt.MapFrom(src => src.Person != null ? src.Person.LastName : null))
                .ForPath(dest => dest.Person.Email,
                    opt => opt.MapFrom(src => src.Person != null ? src.Person.Email : null))
                .ForPath(dest => dest.Person.Phone,
                    opt => opt.MapFrom(src => src.Person != null ? src.Person.Phone : null))
                .ForPath(dest => dest.Person.Username,
                    opt => opt.MapFrom(src => src.Person != null ? src.Person.Username : null))
                .ForMember(dest => dest.CompanyName,
                    opt => opt.MapFrom(src => src.CompanyName ?? string.Empty))
                .ForMember(dest => dest.JobTypeName,
                    opt => opt.MapFrom(src => src.JobTypeName))
                .ForMember(dest => dest.IsFreelancer,
                   opt => opt.MapFrom(src => src.IsFreelancer))
                .ForMember(dest => dest.Locations,
                    opt => opt.MapFrom(src => src.Locations));

            CreateMap<CreateContractorVm, CreateContractorDto>()
                .ForMember(dest => dest.Person, 
                    opt => opt.MapFrom(src => src.Person))
                .ForMember(dest => dest.CompanyName, 
                    opt => opt.MapFrom(src => src.CompanyName ?? string.Empty))
                .ForMember(dest => dest.JobTypeName, 
                    opt => opt.MapFrom(src => src.JobTypeName))
                .ForMember(dest => dest.IsFreelancer, 
                    opt => opt.MapFrom(src => src.IsFreelancer))
                .ForMember(dest => dest.Locations, 
                    opt => opt.Ignore());

            CreateMap<ResponseContractorDto, EditContractorVm>()
                .ConstructUsing(src => new EditContractorVm { Person = new EditPersonVm() })
                .ForPath(dest => dest.Person.Id,
                    opt => opt.MapFrom(src => src.Person != null ? src.Person.Id : 0))
                .ForPath(dest => dest.Person.FirstName,
                    opt => opt.MapFrom(src => src.Person != null ? src.Person.FirstName : null))
                .ForPath(dest => dest.Person.LastName,
                    opt => opt.MapFrom(src => src.Person != null ? src.Person.LastName : null))
                .ForPath(dest => dest.Person.Email,
                    opt => opt.MapFrom(src => src.Person != null ? src.Person.Email : null))
                .ForPath(dest => dest.Person.Phone,
                    opt => opt.MapFrom(src => src.Person != null ? src.Person.Phone : null))
                .ForPath(dest => dest.Person.Username,
                    opt => opt.MapFrom(src => src.Person != null ? src.Person.Username : null))
                .ForMember(dest => dest.JobTypeName,
                    opt => opt.MapFrom(src => src.JobTypeName))
                .ForMember(dest => dest.IsFreelancer,
                    opt => opt.MapFrom(src => src.IsFreelancer))
                .ForMember(dest => dest.SelectedLocationIds,
                    opt => opt.MapFrom(src => src.Locations != null ? src.Locations.Select(l => l.Id).ToList() : new List<int>()));


            CreateMap<EditContractorVm, CreateContractorDto>()
                .ForMember(dest => dest.Person,
                    opt => opt.MapFrom(src => src.Person))
                .ForMember(dest => dest.Locations,
                    opt => opt.Ignore())//postvljam u kontroleru
                .ForMember(dest => dest.CompanyName,
                    opt => opt.MapFrom(src => src.CompanyName ?? string.Empty))
                .ForMember(dest => dest.JobTypeName,
                    opt => opt.MapFrom(src => src.JobTypeName ?? string.Empty))
                .ForMember(dest => dest.IsFreelancer,
                    opt => opt.MapFrom(src => src.IsFreelancer))
                .ForMember(dest => dest.PersonId,
                    opt => opt.MapFrom(src => src.Person != null ? src.Person.Id : 0));
        }
    }
}
