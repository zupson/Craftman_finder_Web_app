using AutoMapper;
using BL.Dtos;
using WebApp.ViewModels;

namespace WebApp.AutoMapper
{
    public class JobApplicationMappingProfile : Profile
    {
        public JobApplicationMappingProfile()
        {
            CreateMap<CreateJobApplicationVm, CreateJobApplicationDto>();
            CreateMap<EditJobApplicationVm, ResponseJobApplicationDto>();

            CreateMap<EditJobApplicationVm, ResponseJobApplicationDto>()
                .ForMember(dest => dest.JobPost,
                opt => opt.MapFrom(src => src.JobPostId));

            CreateMap<ResponseJobApplicationVm, ResponseJobApplicationDto>()
                 .ForMember(dest => dest.JobPost,
                    opt => opt.MapFrom(src => new ResponseJobPostDto { Id = src.JobPost.Id }))
                 .ForMember(dest => dest.CreatedAt,
                    opt => opt.Ignore());

            CreateMap<ResponseJobApplicationDto, ResponseJobApplicationVm>()
                 .ForMember(dest => dest.JobPost, 
                 opt => opt.MapFrom(src => src.JobPost));
        }
    }
}
