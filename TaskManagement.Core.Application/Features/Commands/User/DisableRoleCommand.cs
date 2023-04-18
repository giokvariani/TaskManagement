using FluentValidation;
using MediatR;
using TaskManagement.Core.Application.Exceptions;
using TaskManagement.Core.Application.Interfaces;

namespace TaskManagement.Core.Application.Features.Commands.User
{
    public class DisableRoleCommand : IRequest<int>
    {
        public int UserId { get; }
        public int RoleId { get; }
        public DisableRoleCommand(int userId, int roleId)
        {
            UserId = userId;
            RoleId = roleId;
        }
        public class DisableRoleCommandHandler : IRequestHandler<DisableRoleCommand, int>
        {
            public DisableRoleCommandHandler(IUser2RoleRepository user2RoleRepository, IUserRepository userRepository)
            {
                _user2RoleRepository = user2RoleRepository;
                _userRepository = userRepository;
            }
            private readonly IUserRepository _userRepository;
            private readonly IUser2RoleRepository _user2RoleRepository;
            public async Task<int> Handle(DisableRoleCommand request, CancellationToken cancellationToken)
            {
                var user = (await _userRepository.GetAsync(x => x.Id == request.UserId)).SingleOrDefault();
                if (user == null)
                    throw new EntityNotFoundException(nameof(User));

                var UserRoles = user.Roles.Select(x => x.Role);
                if (UserRoles.All(x => x.Id != request.RoleId))
                    throw new ValidationException("ასეთი როლი იუზერს არ გააჩნია");

                var user2Role = (await _user2RoleRepository.GetAsync(x => x.UserId == request.UserId && x.RoleId == request.RoleId)).SingleOrDefault();
                if (user2Role == null)
                    throw new EntityNotFoundException();

                var adminUsers = await _user2RoleRepository.GetAsync(x => x.Role.IsAdmin);
                if (adminUsers.Count() == 1 && adminUsers.First().UserId == user2Role.UserId)
                    throw new ValidationException($"{user2Role.User.UserName} არის ერთადერთი ადმინი სისტემაში");

                var result = await _user2RoleRepository.DeleteAsync(user2Role.Id);
                return result;
            }
        }
    }
}
