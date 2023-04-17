using AutoMapper;
using TaskManagement.Core.Application.Dtos;
using TaskManagement.Core.Application.Interfaces;

namespace TaskManagement.Core.Application.Features.Commands.User
{
    public class UpdateUserCommand : AlterUserCommand
    {
        public UpdateUserCommand(IdempotentUserDto user) : base(user) { }
        public IdempotentUserDto User { get; }
        public class UpdateUserCommandHandler : AlterUserCommandHandler
        {
            public UpdateUserCommandHandler(IUserRepository userRepository, IMapper mapper) : base(userRepository, mapper) { }
            public async override Task<int> Handle(AlterUserCommand request, CancellationToken cancellationToken)
            {
                await CheckExistingUser(x => x.UserName == request.User.UserName, request.User.UserName);
                await CheckExistingUser(x => x.Email == request.User.Email, request.User.Email);
                var user = _mapper.Map<Domain.Entities.User>(request.User);
                var result = await _userRepository.UpdateAsync(user);
                return result;
            }
        }
    }
}
