using AutoMapper;
using MediatR;
using System.ComponentModel.DataAnnotations;
using System.Linq.Expressions;
using TaskManagement.Core.Application.Dtos;
using TaskManagement.Core.Application.Interfaces;

namespace TaskManagement.Core.Application.Features.Commands.User
{
    public class AlterUserCommand : IRequest<int>
    {
        public UserDto User { get; }
        public AlterUserCommand(UserDto user)
        {
            User = user;
        }
        public abstract class AlterUserCommandHandler : IRequestHandler<AlterUserCommand, int>
        {
            protected readonly IUserRepository _userRepository;
            protected readonly IMapper _mapper;
            protected AlterUserCommandHandler(IUserRepository userRepository, IMapper mapper)
            {
                _userRepository = userRepository;
                _mapper = mapper;
            }
            public abstract Task<int> Handle(AlterUserCommand request, CancellationToken cancellationToken);
            protected async Task CheckExistingUser(Expression<Func<Domain.Entities.User, bool>> predicate, string targetIdentifier)
            {
                var potentialExistingUser = (await _userRepository.GetAsync(predicate)).SingleOrDefault();
                if (potentialExistingUser != null)
                    throw new ValidationException($"{targetIdentifier} უკვე გამოყენებულია!");
            }
        }
    }
}
