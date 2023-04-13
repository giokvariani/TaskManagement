namespace TaskManagement.Core.Domain.Entities
{
    public class User2Role : BaseEnitty
    {
        public int UserId { get; set; }
        public User User { get; set; }
        public int RoleId { get; set; }
        public Role Role { get; set; }
    }
}
