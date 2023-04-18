using AutoMapper;
using MediatR;
using TaskManagement.Core.Application.Dtos;
using TaskManagement.Core.Application.Exceptions;
using TaskManagement.Core.Application.Interfaces;

namespace TaskManagement.Core.Application.Features.Queries.User
{
    public class GetUserQuery : IRequest<IdentifierUserDto>
    {
        public int UserId { get; }
        public GetUserQuery(int userId)
        {
            UserId = userId;
        }
        public class GetUserQueryHandler : IRequestHandler<GetUserQuery, IdentifierUserDto>
        {
            private readonly IUserRepository _userRepository;
            private readonly IMapper _mapper;
            public GetUserQueryHandler(IUserRepository userRepository, IMapper mapper)
            {
                _userRepository = userRepository;
                _mapper = mapper;
            }
            public async Task<IdentifierUserDto> Handle(GetUserQuery request, CancellationToken cancellationToken)
            {
                var user = await _userRepository.GetAsync(request.UserId);
                if (user == null)
                    throw new EntityNotFoundException();
                var idempotentUser = _mapper.Map<IdentifierUserDto>(user);
                return idempotentUser;
            }
        }
    }
}
