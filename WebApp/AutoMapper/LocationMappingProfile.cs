using AutoMapper;
using BL.Dtos;
using WebApp.ViewModels;

namespace WebApp.AutoMapper

{
    public class LocationMappingProfile : Profile
    {
        public LocationMappingProfile()
        {
            CreateMap<ResponseLocationDto, ResponseLocationVm>();

            CreateMap<CreateLocationVm, CreateLocationDto>()
                .ForMember(dest => dest.Id, 
                opt => opt.MapFrom(src => src.Id == 0 ? null : (int?)src.Id)); ;

            CreateMap<ResponseLocationDto, EditLocationVm>();
            CreateMap<EditLocationVm, EditLocationDto>();
        }
    }
}
