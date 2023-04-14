using TaskManagement.Core.Application.Interfaces;
using TaskManagement.Core.Domain.Entities;
using TaskManagement.Infrastructure.Persistence.DataLayer;

namespace TaskManagement.Infrastructure.Persistence.Repositories
{
    public class PersonalAccessTokenRepository : Repository<PersonalAccessToken>, IPersonalAccessTokenRepository
    {
        public PersonalAccessTokenRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
