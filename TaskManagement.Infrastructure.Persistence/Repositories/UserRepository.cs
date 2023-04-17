using FluentValidation;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using TaskManagement.Core.Application.Exceptions;
using TaskManagement.Core.Application.Interfaces;
using TaskManagement.Core.Domain.Entities;
using TaskManagement.Infrastructure.Persistence.DataLayer;

namespace TaskManagement.Infrastructure.Persistence.Repositories
{
    public class UserRepository : Repository<User>, IUserRepository
    {
        public UserRepository(ApplicationDbContext context) : base(context)
        {
        }
        public IQueryable<User> IncludingData => _context.Users.Include(x => x.Roles).ThenInclude(x => x.Role);
        public async Task CheckExistingUser(Expression<Func<User, bool>> predicate, Tuple<string, string> targetIdentifier)
        {
            var potentialExistingUser = (await GetAsync(predicate)).SingleOrDefault();
            if (potentialExistingUser != null)
                throw new ValidationException($"{targetIdentifier.Item1}:{targetIdentifier.Item2} უკვე გამოყენებულია!");
        }
        public async Task<int> DefineRole(int userId, int roleId)
        {
            var user2Role = new User2Role()
            {
                RoleId = roleId,
                UserId = userId
            };
            await _context.User2Roles.AddAsync(user2Role);
            return await _context.SaveChangesAsync();
        }
        public async Task<int> DeleteRole(int userId, int roleId)
        {
            var user2Role = _context.User2Roles.SingleOrDefault(x => x.RoleId == roleId && x.UserId == userId);
            if (user2Role == null)
                throw new EntityNotFoundException("ჩანაწერი არ მოიძებნა");

            _context.User2Roles.Remove(user2Role);
            return await _context.SaveChangesAsync();
        }
        public override async Task<IEnumerable<User>> GetAsync(Expression<Func<User, bool>> predicate)
        {
            return await IncludingData.Where(predicate).ToListAsync();
        }
    }
}
