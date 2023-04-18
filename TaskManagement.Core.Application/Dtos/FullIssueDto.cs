namespace TaskManagement.Core.Application.Dtos
{
    public class FullIssueDto : UpdateIssueDto
    {
        public int ReporterId { get; set; }
    }
}
