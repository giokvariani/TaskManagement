using AutoMapper;
using MediatR;
using TaskManagement.Core.Application.Dtos;
using TaskManagement.Core.Application.Interfaces;


namespace TaskManagement.Core.Application.Features.Commands.Role
{
    public class CreateRoleCommand : IRequest<int>
    {
        public RoleDto Role { get; }
        public CreateRoleCommand(RoleDto role) => Role = role;
        public class CreateRoleCommandHandler : IRequestHandler<CreateRoleCommand, int>
        {
            private readonly IRoleRepository _roleRepository;
            private readonly IMapper _mapper;
            public CreateRoleCommandHandler(IRoleRepository roleRepository, IMapper mapper)
            {
                _roleRepository = roleRepository;
                _mapper = mapper;
            }
            public async Task<int> Handle(CreateRoleCommand request, CancellationToken cancellationToken)
            {
                await _roleRepository.CheckExisting(x => 
                x.Name == request.Role.Name, 
                Tuple.Create(nameof(request.Role.Name), request.Role.Name));

                var role = _mapper.Map<Domain.Entities.Role>(request.Role);
                var result = await _roleRepository.CreateAsync(role);
                return role.Id;
            }
        }
    }
}
