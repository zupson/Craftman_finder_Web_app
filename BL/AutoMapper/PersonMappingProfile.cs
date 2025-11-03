using AutoMapper;
using BL.Dtos;
using BL.Models;

namespace BL.AutoMapper
{
    public class PersonMappingProfile : Profile
    {
        public PersonMappingProfile()
        {
            CreateMap<RegisterPersonDto, Person>()
                .ForMember(dest => dest.Id,
                    opt => opt.Ignore()) 
                .ForMember(dest => dest.PasswordHash,
                    opt => opt.Ignore())
                .ForMember(dest => dest.PasswordSalt,
                    opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt,
                    opt => opt.Ignore()) 
                .ForMember(dest => dest.IsDeleted,
                    opt => opt.Ignore())
                .ForMember(dest => dest.Contractor,
                    opt => opt.Ignore())
                .ForMember(dest => dest.JobApplications,
                    opt => opt.Ignore())
                .ForMember(dest => dest.Roles,
                    opt => opt.Ignore()); ;

            CreateMap<Person, ResponsePersonDto>()
                 .ForMember(dest => dest.Roles, 
                    opt => opt.MapFrom(src => src.Roles)); 

            CreateMap<LoginPersonDto, ResponseLoginPersonDto>();

            CreateMap<Person, ResponseLoginPersonDto>()
                .ForMember(dest => dest.Roles, 
                    opt => opt.MapFrom(src => src.Roles));

            CreateMap<EditPersonDto, Person>()
                .ForMember(dest => dest.PasswordHash, 
                    opt => opt.Ignore()) 
                .ForMember(dest => dest.Roles, 
                    opt => opt.Ignore()); 

            CreateMap<ResponsePersonDto, EditPersonDto >()
                .ForMember(dest => dest.Password, 
                    opt => opt.Ignore());

            CreateMap<RegisterPersonDto, ResponsePersonDto>();

            CreateMap<ResponsePersonDto,Person>()
                .ForMember(dest => dest.PasswordHash, 
                    opt => opt.Ignore())
                .ForMember(dest => dest.PasswordSalt, 
                    opt => opt.Ignore())
                .ForMember(dest => dest.IsDeleted, 
                    opt => opt.Ignore())
                .ForMember(dest => dest.Contractor, 
                    opt => opt.Ignore())
                .ForMember(dest => dest.JobApplications, 
                    opt => opt.Ignore())
                .ForMember(dest => dest.Roles, 
                    opt => opt.MapFrom(src => src.Roles.Select(r => new Role { Id = r.Id, Name = r.Name }).ToList())); 
        }
    }
}
