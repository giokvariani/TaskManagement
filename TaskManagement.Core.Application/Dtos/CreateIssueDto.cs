namespace TaskManagement.Core.Application.Dtos
{
    public class CreateIssueDto
    {
        public string Title { get; set; }
        public string Descritpion { get; set; }
        public int AssigneeId { get; set; }
    }
}
