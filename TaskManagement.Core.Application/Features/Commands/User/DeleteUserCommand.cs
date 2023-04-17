using MediatR;
using TaskManagement.Core.Application.Exceptions;
using TaskManagement.Core.Application.Interfaces;

namespace TaskManagement.Core.Application.Features.Commands.User
{
    public class DeleteUserCommand : IRequest<int>
    {
        public int UserId { get; }
        public DeleteUserCommand(int userId)
        {
            UserId = userId;
        }
        public class DeleteUserCommandHandler : IRequestHandler<DeleteUserCommand, int>
        {
            private readonly IUserRepository _userRepository;
            public DeleteUserCommandHandler(IUserRepository userRepository)
            {
                _userRepository = userRepository;
            }
            public async Task<int> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
            {
                var user = await _userRepository.GetAsync(request.UserId);
                if (user == null)
                    throw new EntityNotFoundException();
                var result = await _userRepository.DeleteAsync(request.UserId);
                return result;
            }
        }
    }
}
