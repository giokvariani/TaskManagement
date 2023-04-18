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
            CreateMap<User, IdentifierUserDto>().ReverseMap();
            CreateMap<Role, RoleDto>().ReverseMap();
            CreateMap<Role, IdentifierRoleDto>().ReverseMap();
            CreateMap<Issue, IssueDto>().ReverseMap();
            CreateMap<Issue, IdentifierIssueDto>().ReverseMap();
            CreateMap<IssueDto, CreateIssueDto>().ReverseMap();
            CreateMap<Issue, UpdateIssueDto>().ReverseMap();

        }
    }
}
