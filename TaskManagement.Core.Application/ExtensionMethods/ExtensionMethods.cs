using System.Security.Claims;
using TaskManagement.Core.Application.Interfaces;
using TaskManagement.Core.Domain.Entities;

namespace TaskManagement.Core.Application.ExtensionMethods
{
    public static class ExtensionMethods
    {
        public static async Task<User> MapToDatabase(this ClaimsPrincipal claimsPrincipal, IUserRepository userRepository)
        {
            var userName = claimsPrincipal.Claims.SingleOrDefault(x => x.Type == "UserName")?.Value;
            var password = claimsPrincipal.Claims.SingleOrDefault(x => x.Type == "Password")?.Value;
            if (userName == null && password == null)
                throw new InvalidOperationException("Unknown user is detected");
            var user = (await userRepository.GetAsync(x => x.UserName == userName && x.Password == password)).Single()!;
            return user;
        }
    }
}
