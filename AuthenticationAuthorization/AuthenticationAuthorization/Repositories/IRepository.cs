using AuthenticationAuthorization.Entities;
using System.Linq.Expressions;

namespace AuthenticationAuthorization.Repositories
{
    public interface IRepository<TEntity, TKey> 
        where TEntity : class, IEntity<TKey>
        where TKey : IComparable<TKey>
    {
        public Task<TEntity?> GetByIdAsync(Expression<Func<TEntity, bool>> predicate);
        public Task<TEntity?> GetByEmailAsync(Expression<Func<TEntity, bool>> predicate);
        public Task<IEnumerable<TEntity>> GetAllAsync();
        public Task UpdateAsync(TEntity user);
        public Task DeleteAsync(TEntity user, CancellationToken cancellationToken = default);
        public Task<bool> ExistsAsync(Expression<Func<TEntity, bool>> filter);
    }
}