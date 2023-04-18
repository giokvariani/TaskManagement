using AutoMapper;
using MediatR;
using TaskManagement.Core.Application.Dtos;
using TaskManagement.Core.Application.Interfaces;

namespace TaskManagement.Core.Application.Features.Queries.Issue
{
    public class GetIssuesQuery : IRequest<IReadOnlyCollection<IdempotentIssueDto>>
    {
        public class GetIssuesQueryHandler : IRequestHandler<GetIssuesQuery, IReadOnlyCollection<IdempotentIssueDto>>
        {
            private readonly IIssueRepository _issueRepository;
            private readonly IMapper _mapper;
            public GetIssuesQueryHandler(IIssueRepository issueRepository, IMapper mapper)
            {
                _issueRepository = issueRepository;
                _mapper = mapper;
            }
            public async Task<IReadOnlyCollection<IdempotentIssueDto>> Handle(GetIssuesQuery request, CancellationToken cancellationToken)
            {
                var issues = await _issueRepository.GetAsync();
                var idempotentIssuesDto = issues.Select(x => _mapper.Map<IdempotentIssueDto>(x));
                return idempotentIssuesDto.ToList().AsReadOnly();
            }
        }
    }
}
