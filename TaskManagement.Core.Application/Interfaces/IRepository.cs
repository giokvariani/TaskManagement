using System.Linq.Expressions;
using TaskManagement.Core.Domain.Entities;

namespace TaskManagement.Core.Application.Interfaces
{
    public interface IRepository<TEntity> where TEntity : BaseEntity
    {
        Task<int> CreateAsync(TEntity entity);
        Task<TEntity?> GetAsync(int id);
        Task<IEnumerable<TEntity>> GetAsync();
        Task<IEnumerable<TEntity>> GetAsync(Expression<Func<TEntity, bool>> predicate);

        Task<int> UpdateAsync(TEntity entity);
        Task<int> DeleteAsync(int id);
        Task<int> DeleteAsync(TEntity entity);
        Task<int> DeleteRangeAsync(IEnumerable<TEntity> entity);

        Task<bool> CheckAsync(Expression<Func<TEntity, bool>> predicate);

        Task<int> CountAsync(Expression<Func<TEntity, bool>> predicate);
        Task CheckExisting(Expression<Func<TEntity, bool>> predicate, Tuple<string, string> targetIdentifier);
    }
}
