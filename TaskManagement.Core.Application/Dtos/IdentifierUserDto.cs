namespace TaskManagement.Core.Application.Dtos
{
    public class IdentifierUserDto : UserDto
    {
        public int Id { get; set; }
        public List<IdentifierRoleDto> Roles { get; set; }
    }
}
