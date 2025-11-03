using AutoMapper;
using BL.Dtos;
using WebApp.ViewModels;

namespace WebApp.AutoMapper

{
    public class PersonMappingProfile : Profile
    {
        public PersonMappingProfile()
        {

            CreateMap<PersonVm, RegisterPersonDto>();

            CreateMap<ResponsePersonDto, EditPersonVm>();

            CreateMap<EditPersonVm, ResponsePersonDto>();

            CreateMap<EditPersonVm, EditPersonDto>()
                .ForMember(dest => dest.Id, 
                opt => opt.MapFrom(src => src.Id)); 

            CreateMap<RegisterPersonDto, RegisterVm>()
                .ForMember(dest => dest.Id,
                    opt => opt.Ignore()); 

            CreateMap<RegisterVm, RegisterPersonDto>();

            CreateMap<LoginVm, LoginPersonDto>();

            CreateMap<EditPersonDto, EditPersonVm>();

            CreateMap<ResponsePersonDto, AdminProfileVm>();
                


        }
    }
}
