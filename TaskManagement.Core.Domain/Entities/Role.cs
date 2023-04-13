namespace TaskManagement.Core.Domain.Entities
{
    public class Role : BaseEntity
    {
        public string Name { get; set; }
        public ICollection<User2Role> Users { get; set; }
    }
}
