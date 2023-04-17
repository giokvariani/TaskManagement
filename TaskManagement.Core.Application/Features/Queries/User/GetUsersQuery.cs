using AutoMapper;
using MediatR;
using TaskManagement.Core.Application.Dtos;
using TaskManagement.Core.Application.Interfaces;

namespace TaskManagement.Core.Application.Features.Queries.User
{
    public class GetUsersQuery : IRequest<IReadOnlyCollection<IdempotentUserDto>>
    {
        public class GetUsersQueryHandler : IRequestHandler<GetUsersQuery, IReadOnlyCollection<IdempotentUserDto>>
        {
            private readonly IUserRepository _userRepository;
            private readonly IMapper _mapper;
            public GetUsersQueryHandler(IUserRepository userRepository, IMapper mapper)
            {
                _userRepository = userRepository;
                _mapper = mapper;
            }
            public async Task<IReadOnlyCollection<IdempotentUserDto>> Handle(GetUsersQuery request, CancellationToken cancellationToken)
            {
                var users = await _userRepository.GetAsync();
                var idempotentUsersDto = users.Select(x => _mapper.Map<IdempotentUserDto>(x));
                return idempotentUsersDto.ToList().AsReadOnly();
            }
        }
    }
}
