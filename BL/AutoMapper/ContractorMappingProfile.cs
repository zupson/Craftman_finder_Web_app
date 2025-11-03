using AutoMapper;
using BL.Dtos;
using BL.Models;

namespace BL.AutoMapper
{
    public class ContractorMappingProfile : Profile
    {
        public ContractorMappingProfile()
        {
            // Mapiranje CreateContractorDto -> Contractor
            CreateMap<CreateContractorDto, Contractor>()
                .ForMember(dest => dest.Id,
                    opt => opt.Ignore())  // Id generira DB
                .ForMember(dest => dest.JobTypeId,
                    opt => opt.Ignore())
                .ForMember(dest => dest.JobType,
                    opt => opt.Ignore())
                //.ForMember(dest => dest.PersonId,
                //    opt => opt.MapFrom(src => src.PersonId))
                .ForMember(dest => dest.PersonId,
                   opt => opt.Ignore())
                //.ForMember(dest => dest.Person,
                //    opt => opt.MapFrom(src => src.Person))
                .ForMember(dest => dest.Person,
                    opt => opt.Ignore())
                .ForMember(dest => dest.CompanyName,
                    opt => opt.MapFrom(src => src.CompanyName))
                .ForMember(dest => dest.IsFreelancer,
                    opt => opt.MapFrom(src => src.IsFreelancer))
                .ForMember(dest => dest.ContractorLocations,
                    opt => opt.Ignore())
                .ForMember(dest => dest.IsDeleted,
                    opt => opt.Ignore());

            // Mapiranje Contractor -> ResponseContractorDto
            CreateMap<Contractor, ResponseContractorDto>()
                .ForMember(dest => dest.JobTypeName,
                    opt => opt.MapFrom(src => src.JobType.Name))
                .ForMember(dest => dest.Person,
                    opt => opt.MapFrom(src => src.Person))
                .ForMember(dest => dest.Locations,
                    opt => opt.MapFrom(src => src.ContractorLocations));


            CreateMap<EditContractorDto, Contractor>()
                .ForMember(dest => dest.Id,
                    opt => opt.Ignore())
                .ForMember(dest => dest.JobTypeId,
                    opt => opt.Ignore()) // postavlja se u servisu
                .ForMember(dest => dest.JobType,
                    opt => opt.Ignore())
                .ForMember(dest => dest.PersonId,
                    opt => opt.MapFrom(src => src.PersonId))
                .ForMember(dest => dest.Person,
                    opt => opt.Ignore()) // uređuje se zasebno u servisu (EditPersonDto)
                .ForMember(dest => dest.ContractorLocations,
                    opt => opt.Ignore())
                .ForMember(dest => dest.IsDeleted,
                    opt => opt.Ignore());
        }
    }
}
