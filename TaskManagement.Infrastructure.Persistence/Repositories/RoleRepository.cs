using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using TaskManagement.Core.Application.Interfaces;
using TaskManagement.Core.Domain.Entities;
using TaskManagement.Infrastructure.Persistence.DataLayer;

namespace TaskManagement.Infrastructure.Persistence.Repositories
{
    public class RoleRepository : Repository<Role>, IRoleRepository
    {
        public RoleRepository(ApplicationDbContext context) : base(context)
        {
        }

        public override async Task<IEnumerable<Role>> GetAsync(Expression<Func<Role, bool>> predicate)
        {
            return await IncludingData.Where(predicate).ToListAsync();
        }
        public IQueryable<Role> IncludingData => _context.Roles.Include(x => x.Users).ThenInclude(x => x.User);
    }
}
