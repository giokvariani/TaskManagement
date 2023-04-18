using TaskManagement.Core.Domain.Entities;

namespace TaskManagement.Core.Application.Services
{
    public interface IAuthorizeService
    {
        Task<string> GenerateToken(string identifier, string password);
        Task<string> ShowValidToken(string identifier, string password);
    }
}
