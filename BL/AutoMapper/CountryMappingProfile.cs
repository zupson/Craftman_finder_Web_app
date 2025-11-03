using AutoMapper;
using BL.Dtos;
using BL.Models;

namespace BL.AutoMapper
{
    public class CountryMappingProfile : Profile
    {
        public CountryMappingProfile()
        {
            CreateMap<CreateCountryDto, Country>()
                .ForMember(dest => dest.Id, 
                    opt => opt.Ignore())
                .ForMember(dest => dest.Towns, 
                    opt => opt.Ignore()); ;

            CreateMap<EditCountryDto, Country>()
                .ForMember(dest => dest.Id, 
                    opt => opt.Ignore())
                .ForMember(dest => dest.Towns, 
                    opt => opt.Ignore());

            CreateMap<Country, ResponseCountryDto>();
        }
    }
}
