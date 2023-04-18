using AutoMapper;
using MediatR;
using TaskManagement.Core.Application.Dtos;
using TaskManagement.Core.Application.Interfaces;

namespace TaskManagement.Core.Application.Features.Commands.Role
{
    public class UpdateRoleCommand : IRequest<int>
    {
        public UpdateRoleCommand(IdempotentRoleDto role)
        {
            Role = role;
        }
        public IdempotentRoleDto Role { get; }
        public class UpdateRoleCommandHandler : IRequestHandler<UpdateRoleCommand, int>
        {
            private readonly IRoleRepository _roleRepository;
            private readonly IMapper _mapper;
            public UpdateRoleCommandHandler(IRoleRepository roleRepository, IMapper mapper)
            {
                _roleRepository = roleRepository;
                _mapper = mapper;
            }
            public async Task<int> Handle(UpdateRoleCommand request, CancellationToken cancellationToken)
            {
                await _roleRepository.CheckExisting(x =>
                x.Name == request.Role.Name,
                Tuple.Create(nameof(request.Role.Name), request.Role.Name));

                var role = _mapper.Map<Domain.Entities.Role>(request.Role);
                var result = await _roleRepository.UpdateAsync(role);
                return result;
            }
        }
    }
}
