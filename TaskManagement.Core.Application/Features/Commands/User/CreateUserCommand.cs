using AutoMapper;
using MediatR;
using System.ComponentModel.DataAnnotations;
using System.Linq.Expressions;
using TaskManagement.Core.Application.Dtos;
using TaskManagement.Core.Application.Interfaces;


namespace TaskManagement.Core.Application.Features.Commands.User
{
    public class CreateUserCommand : AlterUserCommand
    {
        public UserDto User { get; }
        public CreateUserCommand(UserDto user) : base(user)
        {
            User = user;
        }
        public class CreateUserCommandHandler : AlterUserCommandHandler
        {
            public CreateUserCommandHandler(IUserRepository userRepository, IMapper mapper) : base(userRepository, mapper) { }
            public async override Task<int> Handle(AlterUserCommand request, CancellationToken cancellationToken)
            {
                await CheckExistingUser(x => x.UserName == request.User.UserName, request.User.UserName);
                await CheckExistingUser(x => x.Email == request.User.Email, request.User.Email);
                var user = _mapper.Map<Domain.Entities.User>(request.User);
                var result = await _userRepository.CreateAsync(user);
                return user.Id;
            }
        }
    }
}
