using System.ComponentModel;

namespace TaskManagement.Core.Domain.Entities
{
    public class PersonalAccessToken : BaseEntity
    {
        public string Identifier { get; set; }
        public string Password { get; set; }
        public string Value { get; set; }
        public User User { get; set; }
        public int UserId { get; set; }
    }
}
