using System.Text.Json.Serialization;

namespace TaskManagement.Core.Domain.Entities
{
    public class User2Role : BaseEntity
    {
        public int UserId { get; set; }
        [JsonIgnore]
        public User User { get; set; }
        public int RoleId { get; set; }
        [JsonIgnore]
        public Role Role { get; set; }
    }
}
