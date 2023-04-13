namespace TaskManagement.Core.Domain.Entities
{
    public class User : BaseEnitty
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public ICollection<Issue> ReportedIssues { get; set; }
        public ICollection<Issue> AssignedIssues { get; set; }
        public ICollection<User2Role> Roles { get; set; }

    }
}
