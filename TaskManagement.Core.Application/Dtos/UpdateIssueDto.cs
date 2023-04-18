using TaskManagement.Core.Domain.Enums;

namespace TaskManagement.Core.Application.Dtos
{
    public class UpdateIssueDto : CreateIssueDto
    {
        public IssueStatusType Status { get; set; }
        public int Id { get; set; }
    }
}
