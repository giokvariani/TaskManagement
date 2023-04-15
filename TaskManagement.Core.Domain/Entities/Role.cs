using TaskManagement.Core.Domain.Enums;

namespace TaskManagement.Core.Domain.Entities
{
    public class Role : BaseEntity
    {
        public string Name { get; set; }
        public PermissionType Persmissions { get; set; }
        public ICollection<User2Role> Users { get; set; }
    }
}
