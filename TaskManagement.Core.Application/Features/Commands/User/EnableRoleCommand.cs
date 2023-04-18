using FluentValidation;
using MediatR;
using TaskManagement.Core.Application.Exceptions;
using TaskManagement.Core.Application.Interfaces;

namespace TaskManagement.Core.Application.Features.Commands.User
{
    public class EnableRoleCommand : IRequest<int>
    {
        public EnableRoleCommand(int userId, int roleId)
        {
            UserId = userId;
            RoleId = roleId;
        }
        public int UserId { get; }
        public int RoleId { get; }
        public class EnableRoleCommandHandler : IRequestHandler<EnableRoleCommand, int>
        {
            private readonly IUser2RoleRepository _user2RoleRepository;
            private readonly IUserRepository _userRepository;
            private readonly IRoleRepository _roleRepository;
            public EnableRoleCommandHandler(IUser2RoleRepository user2RoleRepository, IUserRepository userRepository, IRoleRepository roleRepository)
            {
                _user2RoleRepository = user2RoleRepository;
                _userRepository = userRepository;
                _roleRepository = roleRepository;   
            }
            public async Task<int> Handle(EnableRoleCommand request, CancellationToken cancellationToken)
            {
                var user = (await _userRepository.GetAsync(x => x.Id == request.UserId)).SingleOrDefault();
                if (user == null)
                    throw new EntityNotFoundException(nameof(User));

                var roles = await _roleRepository.GetAsync();
                if (roles.All(x => x.Id != request.RoleId))
                    throw new EntityNotFoundException(nameof(Role));


                var user2Role = (await _user2RoleRepository.GetAsync(x => x.UserId == request.UserId && x.RoleId == request.RoleId)).SingleOrDefault();
                if (user2Role != null)
                    throw new ValidationException("იუზერს უკვე აქვს აღნიშნული როლი");

                var result = await _user2RoleRepository.CreateAsync(new Domain.Entities.User2Role() { UserId = request.UserId, RoleId = request.RoleId });
                return result;
            }
        }
    }
}
