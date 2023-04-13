using AutoMapper;
using TaskManagement.Core.Application.Dtos;
using TaskManagement.Core.Domain.Entities;

namespace TaskManagement.Core.Application.Mapping
{
    public class AutoProfile : Profile
    {
        public AutoProfile()
        {
            CreateMap<User, CreateUserDto>().ReverseMap();
            CreateMap<User, IdempotentUserDto>().ReverseMap();
        }
    }
}
