namespace TaskManagement.Core.Application.Dtos
{
    public class IdempotentIssueDto : IssueDto
    {
        public int Id { get; set; }
    }
}
