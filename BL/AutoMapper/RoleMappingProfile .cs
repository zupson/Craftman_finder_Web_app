using AutoMapper;
using BL.Dtos;
using BL.Models;

namespace BL.AutoMapper
{
    public class RoleMappingProfile : Profile
    {
        public RoleMappingProfile()
        {
            CreateMap<CreateRoleDto, Role>()
                .ForMember(dest => dest.Id, 
                    opt => opt.Ignore())
                .ForMember(dest => dest.People, 
                    opt => opt.Ignore());


            CreateMap<EditRoleDto, Role>()
            .ForMember(dest => dest.Id, 
                opt => opt.Ignore())// ne mapiši Id, on se ne mijenja
            .ForMember(dest => dest.People, 
                opt => opt.Ignore());

            CreateMap<Role, ResponseRoleDto>();
            CreateMap<ResponseRoleDto, Role>();

        }
    }
}
