using System.Linq.Expressions;
using TaskManagement.Core.Domain.Entities;

namespace TaskManagement.Core.Application.Interfaces
{
    public interface IUserRepository : IRepository<User>
    {
        Task<int> DeleteRole(int userId, int roleId);
        Task<int> DefineRole(int userId, int roleId);
        IQueryable<User> IncludingData { get; }
    }
}
