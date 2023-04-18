using CSharpFunctionalExtensions;
using System.Security.Claims;
using TaskManagement.Core.Application.Interfaces;
using TaskManagement.Core.Domain.Entities;

namespace TaskManagement.Core.Application.ExtensionMethods
{
    public static class ExtensionMethods
    {
        public static async Task<Maybe<User>> MapToDatabase(this ClaimsPrincipal claimsPrincipal, IUserRepository userRepository)
        {
            var userName = claimsPrincipal.Claims.SingleOrDefault(x => x.Type == "UserName")?.Value;
            var password = claimsPrincipal.Claims.SingleOrDefault(x => x.Type == "Password")?.Value;
            if (userName == null && password == null)
                return Maybe<User>.None;
            var user = (await userRepository.GetAsync(x => x.UserName == userName && x.Password == password)).SingleOrDefault();
            return user ?? Maybe<User>.None;
        }
    }
}
