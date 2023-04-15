namespace TaskManagement.Core.Domain.Enums
{
    [Flags]
    public enum PermissionType
    {
        Get = 1,
        Update = 2,
        Delete = 4,
        Post = 8,
    }
}
