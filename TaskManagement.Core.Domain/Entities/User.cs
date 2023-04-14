namespace TaskManagement.Core.Domain.Entities
{
    public class User : BaseEntity
    {
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public ICollection<Issue> ReportedIssues { get; set; }
        public ICollection<Issue> AssignedIssues { get; set; }
        public ICollection<User2Role> Roles { get; set; }
        //public ICollection<PersonalAccessToken> PersonalAccessTokens { get; set; }

    }
}
