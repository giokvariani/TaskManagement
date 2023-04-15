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
    }
}
