using AutoMapper;
using BL.Dtos;
using BL.Models;

namespace BL.AutoMapper
{
    public class LocationMappingProfile : Profile
    {
        public LocationMappingProfile()
        {
            CreateMap<CreateLocationDto, Location>()
                .ForMember(dest => dest.Id,
                    opt => opt.Ignore())
                .ForMember(dest => dest.TownId,
                    opt => opt.Ignore())
                .ForMember(dest => dest.Town,
                    opt => opt.Ignore())
                .ForMember(dest => dest.PostalCode,
                    opt => opt.MapFrom(src => src.PostalCode));

            CreateMap<Location, ResponseLocationDto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.PostalCode, opt => opt.MapFrom(src => src.PostalCode))
                .ForMember(dest => dest.TownId, opt => opt.MapFrom(src => src.TownId))
                .ForMember(dest => dest.TownName, opt => opt.MapFrom(src => src.Town.Name))
                .ForMember(dest => dest.CountryName, opt => opt.MapFrom(src => src.Town.Country.Name));

            CreateMap<EditLocationDto, Location>()
                .ForMember(dest => dest.Town, 
                    opt => opt.Ignore())
                .ForMember(dest => dest.Id, 
                    opt => opt.Ignore())
                .ForMember(dest => dest.IsDeleted, 
                    opt => opt.Ignore());

            CreateMap<CreateLocationDto, ResponseLocationDto>();


            CreateMap<ResponseLocationDto, CreateLocationDto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.PostalCode, opt => opt.MapFrom(src => src.PostalCode))
                .ForMember(dest => dest.TownName, opt => opt.MapFrom(src => src.TownName))
                .ForMember(dest => dest.CountryName, opt => opt.MapFrom(src => src.CountryName));

            CreateMap<ResponseLocationDto, EditLocationDto>()
                .ForMember(dest => dest.Id, 
                    opt => opt.MapFrom(src => (int?)src.Id))
                .ForMember(dest => dest.PostalCode, 
                    opt => opt.MapFrom(src => src.PostalCode))
                .ForMember(dest => dest.TownName, 
                    opt => opt.MapFrom(src => src.TownName))
                .ForMember(dest => dest.CountryName, 
                    opt => opt.MapFrom(src => src.CountryName)); 
        }
    }
}
