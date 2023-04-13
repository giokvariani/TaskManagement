namespace TaskManagement.Core.Domain.Entities
{
    public class Role : BaseEnitty
    {
        public string Name { get; set; }
        public ICollection<User2Role> Users { get; set; }
    }
}
