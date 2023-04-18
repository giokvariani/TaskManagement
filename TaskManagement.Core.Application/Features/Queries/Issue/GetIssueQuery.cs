using AutoMapper;
using MediatR;
using TaskManagement.Core.Application.Dtos;
using TaskManagement.Core.Application.Exceptions;
using TaskManagement.Core.Application.Interfaces;

namespace TaskManagement.Core.Application.Features.Queries.Issue
{
    public class GetIssueQuery : IRequest<IdentifierIssueDto>
    {
        public int IssueId { get; }
        public GetIssueQuery(int issueId)
        {
            IssueId = issueId;
        }
        public class GetIssueQueryHandler : IRequestHandler<GetIssueQuery, IdentifierIssueDto>
        {
            private readonly IIssueRepository _issueRepository;
            private readonly IMapper _mapper;
            public GetIssueQueryHandler(IIssueRepository issueRepository, IMapper mapper)
            {
                _issueRepository = issueRepository;
                _mapper = mapper;
            }
            public async Task<IdentifierIssueDto> Handle(GetIssueQuery request, CancellationToken cancellationToken)
            {
                var issue = await _issueRepository.GetAsync(request.IssueId);
                if (issue == null)
                    throw new EntityNotFoundException();
                var idempotentIssue = _mapper.Map<IdentifierIssueDto>(issue);
                return idempotentIssue;
            }
        }
    }
}
