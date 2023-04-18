using AutoMapper;
using MediatR;
using System.Security.Claims;
using TaskManagement.Core.Application.Dtos;
using TaskManagement.Core.Application.Interfaces;


namespace TaskManagement.Core.Application.Features.Commands.Issue
{
    public class CreateIssueCommand : IRequest<int>
    {
        public IssueDto Issue { get; }
        public ClaimsPrincipal User { get; set; }
        public CreateIssueCommand(IssueDto issue, ClaimsPrincipal user)
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
                var userName = request.User.Claims.SingleOrDefault(x => x.Type == "UserName")?.Value;
                var password = request.User.Claims.SingleOrDefault(x => x.Type == "Password")?.Value;
                var user = (await _userRepository.GetAsync(x => x.UserName == userName)).Single();
                request.Issue.ReporterId = user.Id;
                request.Issue.Status = Domain.Enums.IssueStatusType.Open;

                var Issue = _mapper.Map<Domain.Entities.Issue>(request.Issue);
                var result = await _issueRepository.CreateAsync(Issue);
                return Issue.Id;
            }
        }
    }
}
