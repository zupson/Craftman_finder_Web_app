using AutoMapper;
using BL.Dtos;
using BL.Models;

namespace BL.AutoMapper
{
    public class TownMappingProfile : Profile
    {
        public TownMappingProfile()
        {
            CreateMap<CreateTownDto, Town>()
                .ForMember(dest => dest.Id,
                    opt => opt.Ignore())
                .ForMember(dest => dest.CountryId,
                    opt => opt.Ignore())
                .ForMember(dest => dest.Country,
                    opt => opt.Ignore())
                .ForMember(dest => dest.Name,
                    opt => opt.MapFrom(src => src.Name));

            CreateMap<Town, ResponseTownDto>()
                .ForMember(dest => dest.CountryName,
                    opt => opt.MapFrom(src => src.Country.Name));

            CreateMap<EditTownDto, Town>()
                .ForMember(dest => dest.CountryId,
                    opt => opt.Ignore())
                .ForMember(dest => dest.Country,
                    opt => opt.Ignore())
                .ForMember(dest => dest.Id, 
                    opt => opt.Ignore())
                .ForMember(dest => dest.Name, 
                    opt => opt.Condition(
                        src => !string.IsNullOrEmpty(src.Name)));
        }
    }
}
