using AutoMapper;
using BL.Dtos;
using WebApp.ViewModels;

namespace WebApp.AutoMapper
{
    public class ContractorMappingProfile : Profile
    {
        public ContractorMappingProfile()
        {
            CreateMap<ResponseContractorDto, ResponseContractorVm>()
                .ForMember(dest => dest.IsFreelancer,
                           opt => opt.MapFrom(src => src.IsFreelancer ?? false));

            CreateMap<CreateContractorVm, CreateContractorDto>()
                .ForMember(dest => dest.PersonId, 
                    opt => opt.Ignore())
                .ForMember(dest => dest.IsFreelancer, 
                    opt => opt.MapFrom(src => src.IsFreelancer));

            CreateMap<CreateContractorVm, CreateContractorDto>();
                

            CreateMap<ResponseContractorDto, EditContractorVm>()
                .ForMember(dest => dest.IsFreelancer, 
                    opt => opt.MapFrom(src => src.IsFreelancer ?? false))
                .ForMember(dest => dest.SelectedLocationIds, 
                    opt => opt.Ignore())
                .ForMember(dest => dest.AllLocations, 
                    opt => opt.Ignore())
                .ForMember(dest => dest.NewLocation, 
                    opt => opt.Ignore());

            CreateMap<EditContractorVm, EditContractorDto>()
                .ForMember(dest => dest.Id, 
                    opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.IsFreelancer, 
                    opt => opt.MapFrom(src => src.IsFreelancer)); 
        }
    }
}
