using SimpleLinkShrinkLibrary.Core.Domain.Entities;
using System.Linq.Expressions;

namespace SimpleLinkShrinkLibrary.Core.Application.Persistence
{
    public interface IRepository<T> where T : EntityBase
    {
        Task<T> GetByIdAsync(int id);
        Task<T> GetAsync(Expression<Func<T, bool>> query);
        Task<IReadOnlyList<T>> ListAllAsync(Expression<Func<T, bool>> query);
        Task<T> CreateAsync(T entity);
        Task DeleteAsync(T entity);
        Task DeleteRangeAsync(IEnumerable<T> entities);
    }
}