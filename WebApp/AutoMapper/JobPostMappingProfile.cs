using AutoMapper;
using BL.Dtos;
using WebApp.ViewModels;

namespace WebApp.AutoMapper
{
    public class JobPostMappingProfile : Profile
    {
        public JobPostMappingProfile()
        {
            CreateMap<ResponseJobPostDto, ResponseJobPostVm>();

            CreateMap<ResponseJobPostVm, ResponseJobPostDto>();

            CreateMap<ResponseJobPostDto, EditJobPostVm>();

            CreateMap<ResponseJobPostVm, ResponseJobPostDto>();

            CreateMap<CreateJobPostVm, CreateJobPostDto>();

            CreateMap<EditJobPostVm, EditJobPostDto>();
        }
    }
}
