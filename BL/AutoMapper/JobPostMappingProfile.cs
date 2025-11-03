using AutoMapper;
using BL.Dtos;
using BL.Models;

namespace BL.AutoMapper
{
    public class JobPostMappingProfile : Profile
    {
        public JobPostMappingProfile()
        {
            CreateMap<CreateJobPostDto, JobPost>()
             .ForMember(dest => dest.Id, opt => opt.Ignore())  
             .ForMember(dest => dest.ContractorLocationId, opt => opt.Ignore()) 
             .ForMember(dest => dest.ContractorLocation, opt => opt.Ignore())  
             .ForMember(dest => dest.IsDeleted, opt => opt.Ignore())          
             .ForMember(dest => dest.JobApplications, opt => opt.Ignore());   

            CreateMap<EditJobPostDto, JobPost>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.ContractorLocationId, opt => opt.Ignore())
                .ForMember(dest => dest.ContractorLocation, opt => opt.Ignore())
                .ForMember(dest => dest.IsDeleted, opt => opt.Ignore())
                .ForMember(dest => dest.JobApplications, opt => opt.Ignore());

            CreateMap<JobPost, ResponseJobPostDto>();

        }
    }
}
