using AutoMapper;
using BL.Dtos;
using BL.Models;

namespace BL.AutoMapper
{
    public class JobApplicationMappingProfile : Profile
    {
        public JobApplicationMappingProfile()
        {
            CreateMap<CreateJobApplicationDto, JobApplication>()
                .ForMember(dest => dest.Id, 
                    opt => opt.Ignore())          
                .ForMember(dest => dest.CreatedAt, 
                    opt => opt.Ignore())    
                .ForMember(dest => dest.IsDeleted, 
                    opt => opt.Ignore())    
                .ForMember(dest => dest.Person, 
                    opt => opt.Ignore())       
                .ForMember(dest => dest.JobPost, 
                    opt => opt.Ignore());    

            CreateMap<EditJobApplicationDto, JobApplication>()
                .ForMember(dest => dest.Id, 
                    opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, 
                    opt => opt.Ignore())
                .ForMember(dest => dest.IsDeleted, 
                    opt => opt.Ignore())
                .ForMember(dest => dest.Person, 
                    opt => opt.Ignore())
                .ForMember(dest => dest.JobPost, 
                    opt => opt.Ignore());

            CreateMap<JobApplication, ResponseJobApplicationDto>();
        }
    }
}
