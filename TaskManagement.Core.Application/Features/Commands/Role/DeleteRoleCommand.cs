using MediatR;
using TaskManagement.Core.Application.Exceptions;
using TaskManagement.Core.Application.Interfaces;

namespace TaskManagement.Core.Application.Features.Commands.Role
{
    public class DeleteRoleCommand : IRequest<int>
    {
        public int RoleId { get; }
        public DeleteRoleCommand(int roleId)
        {
            RoleId = roleId;
        }
        public class DeleteRoleCommandHandler : IRequestHandler<DeleteRoleCommand, int>
        {
            private readonly IRoleRepository _roleRepository;
            private readonly IUser2RoleRepository _user2roleRepository;
            public DeleteRoleCommandHandler(IRoleRepository roleRepository, IUser2RoleRepository user2RoleRepository)
            {
                _roleRepository = roleRepository;
                _user2roleRepository = user2RoleRepository;

            }
            public async Task<int> Handle(DeleteRoleCommand request, CancellationToken cancellationToken)
            {
                var role = (await _roleRepository.GetAsync(x => x.Id == request.RoleId)).SingleOrDefault();
                if (role == null)
                    throw new EntityNotFoundException();

                var userConnections = role.Users;
                if (userConnections.Any())
                {
                    await _user2roleRepository.DeleteRangeAsync(userConnections);
                }

                var result = await _roleRepository.DeleteAsync(request.RoleId);
                return result;
            }
        }
    }
}
