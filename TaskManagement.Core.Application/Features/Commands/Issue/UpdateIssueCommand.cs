using AutoMapper;
using MediatR;
using TaskManagement.Core.Application.Dtos;
using TaskManagement.Core.Application.Interfaces;

namespace TaskManagement.Core.Application.Features.Commands.Issue
{
    public class UpdateIssueCommand : IRequest<int>
    {
        public UpdateIssueCommand(IdempotentIssueDto issue)
        {
            Issue = issue;
        }
        public IdempotentIssueDto Issue { get; }
        public class UpdateIssueCommandHandler : IRequestHandler<UpdateIssueCommand, int>
        {
            private readonly IIssueRepository _issueRepository;
            private readonly IMapper _mapper;
            public UpdateIssueCommandHandler(IIssueRepository issueRepository, IMapper mapper)
            {
                _issueRepository = issueRepository;
                _mapper = mapper;
            }
            public async Task<int> Handle(UpdateIssueCommand request, CancellationToken cancellationToken)
            {
                var issue = _mapper.Map<Domain.Entities.Issue>(request.Issue);
                var result = await _issueRepository.UpdateAsync(issue);
                return result;
            }
        }
    }
}
