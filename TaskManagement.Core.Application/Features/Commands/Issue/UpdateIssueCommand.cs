using AutoMapper;
using MediatR;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using TaskManagement.Core.Application.Dtos;
using TaskManagement.Core.Application.Exceptions;
using TaskManagement.Core.Application.ExtensionMethods;
using TaskManagement.Core.Application.Interfaces;

namespace TaskManagement.Core.Application.Features.Commands.Issue
{
    public class UpdateIssueCommand : IRequest<int>
    {
        public UpdateIssueCommand(UpdateIssueDto issue, ClaimsPrincipal user)
        {
            Issue = issue;
            User = user;
        }
        public UpdateIssueDto Issue { get; }
        public ClaimsPrincipal User { get; }
        public class UpdateIssueCommandHandler : IRequestHandler<UpdateIssueCommand, int>
        {
            private readonly IUserRepository _userRepository;
            private readonly IIssueRepository _issueRepository;
            private readonly IMapper _mapper;
            public UpdateIssueCommandHandler(IIssueRepository issueRepository, IMapper mapper, IUserRepository userRepository)
            {
                _issueRepository = issueRepository;
                _mapper = mapper;
                _userRepository = userRepository;
            }
            public async Task<int> Handle(UpdateIssueCommand request, CancellationToken cancellationToken)
            {
                var issueFromDb = await _issueRepository.GetAsync(request.Issue.Id);
                if (issueFromDb == null)
                    throw new EntityNotFoundException();

                var currentUser = await request.User.MapToDatabase(_userRepository);
                var currentUserIsAdmin = currentUser.Roles.Select(x => x.Role).Any(x => x.IsAdmin);

                if (!currentUserIsAdmin && currentUser.Id != issueFromDb.ReporterId && currentUser.Id != issueFromDb.AssigneeId)
                    throw new ValidationException("Access is denied");

                var issue = _mapper.Map<Domain.Entities.Issue>(request.Issue);
                var result = await _issueRepository.UpdateAsync(issue);
                return result;
            }
        }
    }
}
