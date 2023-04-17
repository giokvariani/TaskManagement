using AutoMapper;
using MediatR;
using TaskManagement.Core.Application.Dtos;
using TaskManagement.Core.Application.Interfaces;


namespace TaskManagement.Core.Application.Features.Commands.User
{
    public class CreateUserCommand : IRequest<int>
    {
        public UserDto User { get; }
        public CreateUserCommand(UserDto user) => User = user;
        public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, int>
        {
            private readonly IUserRepository _userRepository;
            private readonly IMapper _mapper;
            public CreateUserCommandHandler(IUserRepository userRepository, IMapper mapper)
            {
                _userRepository = userRepository;
                _mapper = mapper;
            }
            public async Task<int> Handle(CreateUserCommand request, CancellationToken cancellationToken)
            {
                await _userRepository.CheckExisting(x => 
                x.UserName == request.User.UserName, 
                Tuple.Create(nameof(request.User.UserName), request.User.UserName));

                await _userRepository.CheckExisting(x => 
                x.Email == request.User.Email, 
                Tuple.Create(nameof(request.User.Email), request.User.Email));

                var user = _mapper.Map<Domain.Entities.User>(request.User);
                var result = await _userRepository.CreateAsync(user);
                return user.Id;
            }
        }
    }
}
