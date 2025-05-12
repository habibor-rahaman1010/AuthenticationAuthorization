using AuthenticationAuthorization.Entities;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace AuthenticationAuthorization.Repositories
{
    public abstract class Repository<TEntity, TKey> : IRepository<TEntity, TKey>
        where TEntity : class, IEntity<TKey>
        where TKey : IComparable<TKey>
    {
        private readonly DbContext? _dbContext;
        private readonly DbSet<TEntity>? _dbSet;
        public Repository(DbContext dbContext)
        {
            _dbContext = dbContext;
            _dbSet = _dbContext?.Set<TEntity>();
        }

        public virtual async Task DeleteAsync(TEntity entityToDelete, CancellationToken cancellationToken)
        {
            if (_dbSet == null)
            {
                throw new InvalidOperationException("DbSet is not initialized.");
            }
            await Task.Run(() =>
            {
                if (_dbContext.Entry(entityToDelete).State == EntityState.Detached)
                {
                    _dbSet.Attach(entityToDelete);
                }
                _dbSet.Remove(entityToDelete);
            }, cancellationToken);
        }

        public virtual async Task<IEnumerable<TEntity>> GetAllAsync()
        {
            if (_dbSet == null)
            {
                throw new InvalidOperationException("DbSet is not initialized.");
            }
            IQueryable<TEntity> query = _dbSet.AsQueryable<TEntity>();
            return await query.ToListAsync();
        }

        public virtual async Task<TEntity?> GetByEmailAsync(Expression<Func<TEntity, bool>> predicate)
        {
            if (_dbSet == null)
            {
                throw new InvalidOperationException("DbSet is not initialized.");
            }
            IQueryable<TEntity> query = _dbSet.AsQueryable<TEntity>();
            return await query.FirstOrDefaultAsync(predicate);
        }

        public virtual async Task<TEntity?> GetByIdAsync(Expression<Func<TEntity, bool>> predicate)
        {
            if (_dbSet == null)
            {
                throw new InvalidOperationException("DbSet is not initialized.");
            }
            IQueryable<TEntity> query = _dbSet.AsQueryable<TEntity>();
            return await query.FirstOrDefaultAsync(predicate);  
        }

        public virtual async Task<bool> ExistsAsync(Expression<Func<TEntity, bool>> filter)
        {
            if (_dbSet == null)
            {
                throw new InvalidOperationException("DbSet is not initialized.");
            }
            IQueryable<TEntity> query = _dbSet.AsQueryable<TEntity>();
            return await query.AnyAsync(filter);
        }

        public virtual async Task UpdateAsync(TEntity user)
        {
            if (_dbSet == null)
            {
                throw new InvalidOperationException("DbSet is not initialized.");
            }
            await Task.Run(() =>
            {
                _dbSet.Attach(user);
                _dbContext.Entry(user).State = EntityState.Modified;
            });
        }
    }
}
