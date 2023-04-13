﻿using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using TaskManagement.Core.Application.Exceptions;
using TaskManagement.Core.Application.Interfaces;
using TaskManagement.Core.Domain.Entities;
using TaskManagement.Infrastructure.Persistence.DataLayer;

namespace TaskManagement.Infrastructure.Persistence.Repositories
{
    public class Repository<T> : IRepository<T> where T : BaseEntity
    {
        private readonly ApplicationDbContext _context;
        public async Task<bool> CheckAsync(Expression<Func<T, bool>> predicate) => await _context.Set<T>().AnyAsync(predicate);
        public async Task<int> CountAsync(Expression<Func<T, bool>> predicate) => await _context.Set<T>().CountAsync(predicate);
        public async Task<int> CreateAsync(T entity)
        {
            _context.Set<T>().Add(entity);
            return await _context.SaveChangesAsync();
        }

        public async Task<int> DeleteAsync(int id)
        {
            var entity = await GetAsync(id);
            _context.Set<T>().Remove(entity);
            return await _context.SaveChangesAsync();
        }

        public async Task<int> DeleteAsync(T entity)
        {
            _context.Set<T>().Remove(entity);
            return await _context.SaveChangesAsync();
        }

        public async Task<int> DeleteRangeAsync(IEnumerable<T> entity)
        {
            _context.Set<T>().RemoveRange(entity);
            return await _context.SaveChangesAsync();
        }

        public async Task<T?> GetAsync(int id) => await _context.Set<T>().FindAsync(id);

        public async Task<IEnumerable<T>> GetAsync() => await _context.Set<T>().ToListAsync();

        public async Task<IEnumerable<T>> GetAsync(Expression<Func<T, bool>> predicate)
        {
            return await _context.Set<T>().Where(predicate).ToListAsync();
        }

        public async Task<int> UpdateAsync(T entity)
        {
            _context.Set<T>().Update(entity);
            return await _context.SaveChangesAsync();
        }

        public async Task<int> UpdateAsync(int id, T entity)
        {
            var enittyInDatabase = await GetAsync(id);
            if (enittyInDatabase == null)
                throw new EntityNotFoundException("ჩანაწერი ვერ მოიძებნა");
            _context.Entry(enittyInDatabase).CurrentValues.SetValues(entity);
            return await _context.SaveChangesAsync();
        }
    }
}
