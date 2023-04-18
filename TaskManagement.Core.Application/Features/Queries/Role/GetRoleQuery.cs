using AutoMapper;
using MediatR;
using TaskManagement.Core.Application.Dtos;
using TaskManagement.Core.Application.Exceptions;
using TaskManagement.Core.Application.Interfaces;

namespace TaskManagement.Core.Application.Features.Queries.Role
{
    public class GetRoleQuery : IRequest<IdempotentRoleDto>
    {
        public int RoleId { get; }
        public GetRoleQuery(int roleId)
        {
            RoleId = roleId;
        }
        public class GetRoleQueryHandler : IRequestHandler<GetRoleQuery, IdempotentRoleDto>
        {
            private readonly IRoleRepository _roleRepository;
            private readonly IMapper _mapper;
            public GetRoleQueryHandler(IRoleRepository roleRepository, IMapper mapper)
            {
                _roleRepository = roleRepository;
                _mapper = mapper;
            }
            public async Task<IdempotentRoleDto> Handle(GetRoleQuery request, CancellationToken cancellationToken)
            {
                var role = await _roleRepository.GetAsync(request.RoleId);
                if (role == null)
                    throw new EntityNotFoundException();
                var idempotentRole = _mapper.Map<IdempotentRoleDto>(role);
                return idempotentRole;
            }
        }
    }
}
