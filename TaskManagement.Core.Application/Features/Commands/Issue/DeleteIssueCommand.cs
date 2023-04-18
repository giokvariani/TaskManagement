using MediatR;
using TaskManagement.Core.Application.Exceptions;
using TaskManagement.Core.Application.Interfaces;

namespace TaskManagement.Core.Application.Features.Commands.Issue
{
    public class DeleteIssueCommand : IRequest<int>
    {
        public int IssueId { get; }
        public DeleteIssueCommand(int issueId)
        {
            IssueId = issueId;
        }
        public class DeleteIssueCommandHandler : IRequestHandler<DeleteIssueCommand, int>
        {
            private readonly IIssueRepository _issueRepository;
            public DeleteIssueCommandHandler(IIssueRepository issueRepository)
            {
                _issueRepository = issueRepository;
            }
            public async Task<int> Handle(DeleteIssueCommand request, CancellationToken cancellationToken)
            {
                var issue = await _issueRepository.GetAsync(request.IssueId);
                if (issue == null)
                    throw new EntityNotFoundException();
                var result = await _issueRepository.DeleteAsync(request.IssueId);
                return result;
            }
        }
    }
}
