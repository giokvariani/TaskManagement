using AutoMapper;
using MediatR;
using TaskManagement.Core.Application.Dtos;
using TaskManagement.Core.Application.Interfaces;

namespace TaskManagement.Core.Application.Features.Queries.Role
{
    public class GetRolesQuery : IRequest<IReadOnlyCollection<IdempotentRoleDto>>
    {
        public class GetRolesQueryHandler : IRequestHandler<GetRolesQuery, IReadOnlyCollection<IdempotentRoleDto>>
        {
            private readonly IRoleRepository _roleRepository;
            private readonly IMapper _mapper;
            public GetRolesQueryHandler(IRoleRepository roleRepository, IMapper mapper)
            {
                _roleRepository = roleRepository;
                _mapper = mapper;
            }
            public async Task<IReadOnlyCollection<IdempotentRoleDto>> Handle(GetRolesQuery request, CancellationToken cancellationToken)
            {
                var roles = await _roleRepository.GetAsync();
                var idempotentRolesDto = roles.Select(x => _mapper.Map<IdempotentRoleDto>(x));
                return idempotentRolesDto.ToList().AsReadOnly();
            }
        }
    }
}
