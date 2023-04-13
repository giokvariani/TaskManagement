namespace TaskManagement.Core.Domain.Entities
{
    public class Issue : BaseEntity
    {
        public string Title { get; set; }
        public string Descritpion { get; set; }
        public User Reporter { get; set; }
        public int ReporterId { get; set; }
        public User Assignee { get; set; }
        public int AssigneeId { get; set; }
    }
}
