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
            public DeleteRoleCommandHandler(IRoleRepository roleRepository)
            {
                _roleRepository = roleRepository;
            }
            public async Task<int> Handle(DeleteRoleCommand request, CancellationToken cancellationToken)
            {
                var role = await _roleRepository.GetAsync(request.RoleId);
                if (role == null)
                    throw new EntityNotFoundException();
                var result = await _roleRepository.DeleteAsync(request.RoleId);
                return result;
            }
        }
    }
}
