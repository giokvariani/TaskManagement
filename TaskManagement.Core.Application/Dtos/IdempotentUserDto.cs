namespace TaskManagement.Core.Application.Dtos
{
    public class IdempotentUserDto : UserDto
    {
        public int Id { get; set; }
    }
}
