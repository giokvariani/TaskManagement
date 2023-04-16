using AutoMapper;
using TaskManagement.Core.Application.Dtos;
using TaskManagement.Core.Domain.Entities;

namespace TaskManagement.Core.Application.Mapping
{
    public class AutoProfile : Profile
    {
        public AutoProfile()
        {
            CreateMap<User, UserDto>().ReverseMap();
            CreateMap<User, IdempotentUserDto>().ReverseMap();
            CreateMap<Role, RoleDto>().ReverseMap();
            CreateMap<Role, IdempotentRoleDto>().ReverseMap();
            CreateMap<Issue, IssueDto>().ReverseMap();
            CreateMap<Issue, IdempotentIssueDto>().ReverseMap();
        }
    }
}
