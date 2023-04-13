namespace TaskManagement.Core.Domain.Entities
{
    public class Issue : BaseEnitty
    {
        public string Title { get; set; }
        public string Descritpion { get; set; }
        public User Reporter { get; set; }
        public User Assignee { get; set; }
    }
}
