//using MediatR;
//using TaskManagement.Core.Application.Exceptions;
//using TaskManagement.Core.Application.Interfaces;

//namespace TaskManagement.Core.Application.Features.Commands.User
//{
//    public class DeleteUserCommand : IRequest<int>
//    {
//        public int UserId { get; }
//        public DeleteUserCommand(int userId)
//        {
//            UserId = userId;
//        }
//        public class DeleteUserCommandHandler : IRequestHandler<DeleteUserCommand, int>
//        {
//            private readonly IUserRepository _userRepository;
//            private readonly IUser2RoleRepository _user2roleRepository;
//            public DeleteUserCommandHandler(IUserRepository userRepository, IUser2RoleRepository user2roleRepository)
//            {
//                _userRepository = userRepository;
//                _user2roleRepository = user2roleRepository;
//            }
//            public async Task<int> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
//            {
//                var user = (await _userRepository.GetAsync(x => x.Id == request.UserId)).SingleOrDefault();
//                if (user == null)
//                    throw new EntityNotFoundException();

//                var roleConnections = user.Roles.ToList();
//                if (roleConnections.Any())
//                {
//                    await _user2roleRepository.DeleteRangeAsync(roleConnections);
//                }

//                var result = await _userRepository.DeleteAsync(request.UserId);
//                return result;
//            }
//        }
//    }
//}
