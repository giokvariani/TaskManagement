using AutoMapper;
using MediatR;
using System.Security.Claims;
using TaskManagement.Core.Application.Dtos;
using TaskManagement.Core.Application.Exceptions;
using TaskManagement.Core.Application.ExtensionMethods;
using TaskManagement.Core.Application.Interfaces;


namespace TaskManagement.Core.Application.Features.Commands.Issue
{
    public class CreateIssueCommand : IRequest<int>
    {
        public CreateIssueDto Issue { get; }
        public ClaimsPrincipal User { get; }
        public CreateIssueCommand(CreateIssueDto issue, ClaimsPrincipal user)
        {
            Issue = issue;
            User = user;
        }

        public class CreateIssueCommandHandler : IRequestHandler<CreateIssueCommand, int>
        {
            private readonly IIssueRepository _issueRepository;
            private readonly IUserRepository _userRepository;
            private readonly IMapper _mapper;
            public CreateIssueCommandHandler(IIssueRepository issueRepository, IMapper mapper, IUserRepository userRepository)
            {
                _issueRepository = issueRepository;
                _mapper = mapper;
                _userRepository = userRepository;
            }
            public async Task<int> Handle(CreateIssueCommand request, CancellationToken cancellationToken)
            {
                var potentialUser = await request.User.MapToDatabase(_userRepository);
                if (potentialUser.HasNoValue)
                    throw new UnknownUserException();

                var issue = _mapper.Map<Domain.Entities.Issue>(request.Issue);
                issue.ReporterId = potentialUser.Value.Id;
                issue.Status = Domain.Enums.IssueStatusType.Open;

                var result = await _issueRepository.CreateAsync(issue);
                return issue.Id;
            }
        }
    }
}
