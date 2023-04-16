namespace TaskManagement.Core.Application.Dtos
{
    public class IssueDto
    {
        public string Title { get; set; }
        public string Descritpion { get; set; }
        public int ReporterId { get; set; }
        public int AssigneeId { get; set; }
    }
}
