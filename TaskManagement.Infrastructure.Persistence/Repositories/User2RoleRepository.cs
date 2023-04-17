using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using TaskManagement.Core.Application.Interfaces;
using TaskManagement.Core.Domain.Entities;
using TaskManagement.Infrastructure.Persistence.DataLayer;

namespace TaskManagement.Infrastructure.Persistence.Repositories
{
    public class User2RoleRepository : Repository<User2Role>, IUser2RoleRepository
    {
        public User2RoleRepository(ApplicationDbContext context) : base(context)
        {
        }
        public IQueryable<User2Role> IncludingData => _context.User2Roles.Include(x => x.Role).Include(x => x.User);
        public async override Task<IEnumerable<User2Role>> GetAsync(Expression<Func<User2Role, bool>> predicate) => 
            await IncludingData.Where(predicate).ToListAsync();
    }
}
