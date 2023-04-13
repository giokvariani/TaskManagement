//using MediatR;
//using TaskManagement.Core.Application.Dtos;

//namespace TaskManagement.Core.Application.Features.Queries
//{
//    public class GetUserQuery : IRequest<GetUserDto>
//    {
//        public int UserId { get; set; }
//        public GetUserQuery(int userId)
//        {
//            UserId = UserId;
//        }

//        public class GetUserQueryHandler : IRequestHandler<GetUserQuery, GetUserDto>
//        {
//            public Task<GetUserDto> Handle(GetUserQuery request, CancellationToken cancellationToken)
//            {
//            }
//        }
//    }
//}
