using Microsoft.AspNetCore.Authorization;

namespace TaskManagement.API.Attributes
{
    public class AdminPrivilegeAttribute : AuthorizeAttribute
    {
    }
}
