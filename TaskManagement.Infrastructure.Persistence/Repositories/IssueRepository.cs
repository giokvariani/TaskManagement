using TaskManagement.Core.Application.Interfaces;
using TaskManagement.Core.Domain.Entities;
using TaskManagement.Infrastructure.Persistence.DataLayer;

namespace TaskManagement.Infrastructure.Persistence.Repositories
{
    public class IssueRepository : Repository<Issue>, IIssueRepository
    {
        public IssueRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
