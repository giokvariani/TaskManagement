using AutoMapper;
using MediatR;
using TaskManagement.Core.Application.Dtos;
using TaskManagement.Core.Application.Interfaces;

namespace TaskManagement.Core.Application.Features.Commands.User
{
    public class UpdateUserCommand : IRequest<int>
    {
        public UpdateUserCommand(IdempotentUserDto user)
        {
            User = user;
        }
        public IdempotentUserDto User { get; }
        public class UpdateUserCommandHandler : IRequestHandler<UpdateUserCommand, int>
        {
            protected readonly IUserRepository _userRepository;
            protected readonly IMapper _mapper;
            public UpdateUserCommandHandler(IUserRepository userRepository, IMapper mapper)
            {
                _userRepository = userRepository;
                _mapper = mapper;
            }
            public async Task<int> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
            {
                await _userRepository.CheckExistingUser(x => 
                x.UserName == request.User.UserName && x.Id != request.User.Id, 
                Tuple.Create(nameof(request.User.UserName), request.User.UserName) );

                await _userRepository.CheckExistingUser(x => x.Email == request.User.Email && x.Id != request.User.Id, 
                    Tuple.Create(nameof(request.User.Email), request.User.Email));

                var user = _mapper.Map<Domain.Entities.User>(request.User);
                var result = await _userRepository.UpdateAsync(user);
                return result;
            }
        }
    }
}
