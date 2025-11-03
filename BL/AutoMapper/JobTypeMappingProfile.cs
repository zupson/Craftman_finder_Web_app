using AutoMapper;
using BL.Dtos;
using BL.Models;

namespace BL.AutoMapper
{
    public class JobTypeMappingProfile : Profile
    {
        public JobTypeMappingProfile()
        {
            CreateMap<CreateJobTypeDto, JobType>()
                .ForMember(dest=>dest.Id, 
                    opt => opt.Ignore())
                .ForMember(dest=>dest.IsDeleted, 
                    opt => opt.Ignore())
                .ForMember(dest=>dest.Contractors, 
                    opt => opt.Ignore());

            CreateMap<EditJobTypeDto, JobType>()
                .ForMember(dest=>dest.Id, 
                    opt => opt.Ignore())
                .ForMember(dest=>dest.IsDeleted, 
                    opt => opt.Ignore())
                .ForMember(dest=>dest.Contractors, 
                    opt => opt.Ignore());

            CreateMap<JobType, ResponseJobTypeDto>();
        }
    }
}
