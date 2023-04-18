using FluentValidation;
using MediatR;
using System.Security.Claims;
using TaskManagement.Core.Application.Exceptions;
using TaskManagement.Core.Application.ExtensionMethods;
using TaskManagement.Core.Application.Interfaces;

namespace TaskManagement.Core.Application.Features.Commands.Issue
{
    public class DeleteIssueCommand : IRequest<int>
    {
        public int IssueId { get; }
        public ClaimsPrincipal User { get; }
        public DeleteIssueCommand(int issueId, ClaimsPrincipal user)
        {
            IssueId = issueId;
            User = user;
        }
        public class DeleteIssueCommandHandler : IRequestHandler<DeleteIssueCommand, int>
        {
            private readonly IIssueRepository _issueRepository;
            private readonly IUserRepository _userRepository;
            public DeleteIssueCommandHandler(IIssueRepository issueRepository, IUserRepository userRepository)
            {
                _issueRepository = issueRepository;
                _userRepository = userRepository;
            }
            public async Task<int> Handle(DeleteIssueCommand request, CancellationToken cancellationToken)
            {
                var issue = await _issueRepository.GetAsync(request.IssueId);
                if (issue == null)
                    throw new EntityNotFoundException();

                var currentUser = await request.User.MapToDatabase(_userRepository);
                var currentUserIsAdmin = currentUser.Roles.Select(x => x.Role).Any(x => x.IsAdmin);
                
                if (!currentUserIsAdmin && currentUser.Id != issue.ReporterId && currentUser.Id != issue.AssigneeId)
                    throw new ValidationException("Access is denied");

                var result = await _issueRepository.DeleteAsync(request.IssueId);
                return result;
            }
        }
    }
}
