using AutoMapper;
using MediatR;
using System.Security.Claims;
using TaskManagement.Core.Application.Dtos;
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
                var issueDto = _mapper.Map<IssueDto>(request.Issue);
                var user = await request.User.MapToDatabase(_userRepository);

                issueDto.ReporterId = user.Id;
                issueDto.Status = Domain.Enums.IssueStatusType.Open;

                var Issue = _mapper.Map<Domain.Entities.Issue>(issueDto);
                var result = await _issueRepository.CreateAsync(Issue);
                return Issue.Id;
            }
        }
    }
}
