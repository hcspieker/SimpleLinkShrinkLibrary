using Microsoft.EntityFrameworkCore;
using SimpleLinkShrinkLibrary.Core.Application.Persistence;
using SimpleLinkShrinkLibrary.Core.Domain.Entities;
using SimpleLinkShrinkLibrary.Core.Domain.Exceptions;
using System.Linq.Expressions;

namespace SimpleLinkShrinkLibrary.Infrastructure.Persistence.Repositories
{
    public class BaseRepository<T> : IRepository<T> where T : EntityBase
    {
        protected readonly ShortlinkDbContext _dbContext;

        public BaseRepository(ShortlinkDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<T> GetByIdAsync(int id)
        {
            return await _dbContext.Set<T>().FindAsync(id) ?? throw new EntryNotFoundException();
        }

        public async Task<T> GetAsync(Expression<Func<T, bool>> query)
        {
            return await _dbContext.Set<T>().SingleAsync(query);
        }

        public async Task<IReadOnlyList<T>> ListAllAsync(Expression<Func<T, bool>> query)
        {
            return await _dbContext.Set<T>().Where(query).ToListAsync();
        }

        public async Task<T> CreateAsync(T entity)
        {
            await _dbContext.Set<T>().AddAsync(entity);
            await _dbContext.SaveChangesAsync();

            return entity;
        }

        public async Task DeleteAsync(T entity)
        {
            _dbContext.Set<T>().Remove(entity);
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteRangeAsync(IEnumerable<T> entities)
        {
            _dbContext.Set<T>().RemoveRange(entities);
            await _dbContext.SaveChangesAsync();
        }
    }
}
