using AutoMapper;
using BL.Dtos;
using BL.Models;

namespace BL.AutoMapper
{
    public class ContractorLocationMappingProfile : Profile
    {
        public ContractorLocationMappingProfile()
        {
            CreateMap<CreateContractorLocationDto, ContractorLocation>()
                .ForMember(dest => dest.Id,
                    opt => opt.Ignore())
                .ForMember(dest => dest.Contractor,
                    opt => opt.Ignore())
                .ForMember(dest => dest.Location,
                    opt => opt.Ignore())
                .ForMember(dest => dest.JobPosts,
                    opt => opt.Ignore());

            CreateMap<ContractorLocation, ResponseContractorLocationDto>();

            CreateMap<ContractorLocation, ResponseLocationDto>()
                .ForMember(dest => dest.Id, 
                    opt => opt.MapFrom(src => src.Location.Id))
                .ForMember(dest => dest.PostalCode, 
                    opt => opt.MapFrom(src => src.Location.PostalCode))
                .ForMember(dest => dest.TownId, 
                    opt => opt.MapFrom(src => src.Location.Town.Id))
                .ForMember(dest => dest.TownName, 
                    opt => opt.MapFrom(src => src.Location.Town.Name))
                .ForMember(dest => dest.CountryName, 
                    opt => opt.MapFrom(src => src.Location.Town.Country.Name));
        }
    }
}
