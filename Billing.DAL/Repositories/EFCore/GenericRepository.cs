using System.Linq.Expressions;
using Billing.DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Billing.DAL.Repositories.MemoryRepositories
{

    public abstract class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : class
    {
        protected DbSet<TEntity> _db;
        protected DbContext _context;

        protected GenericRepository(DbContext context)
        {
            _db = context.Set<TEntity>();
            _context = context;
        }

        public async Task Add(TEntity item)
        {
            await _db.AddAsync(item);
        }

        public void Delete(TEntity item)
        {
            AttachEntity(item);
            _db.Remove(item);
        }

        public void Update(TEntity item)
        {
            AttachEntity(item);
            _context.Entry(item).State = EntityState.Modified;
        }

        public async Task<bool> Any(Expression<Func<TEntity, bool>> predicate)
        {
            return await _db.AnyAsync(predicate);
        }

        private void AttachEntity(TEntity item)
        {
            if (_context.Entry(item).State == EntityState.Detached)
            {
                _db.Attach(item);
            }
        }

        public async Task<TEntity?> Get(long id)
        {
            return await _db.FindAsync(id);
        }

        public async Task<TEntity?> GetWithInclude(long id, params Expression<Func<TEntity, object>>[] includeProperties)
        {

            var idName = _context.Model.FindEntityType(typeof(TEntity))!
                                  .FindPrimaryKey()!.Properties
                                  .Select(p => p.Name)
                                  .FirstOrDefault();
            var parameter = Expression.Parameter(typeof(TEntity));
            var property = Expression.Property(parameter, idName!);
            var byId = Expression.Lambda<Func<TEntity, bool>>(Expression.Equal(
                property, Expression.Constant(id)), parameter);

            TEntity? value = await Include(includeProperties).SingleOrDefaultAsync(byId);
            return value;
        }

        public virtual IQueryable<TEntity> GetAllAsQueryable()
        {
            return _db.AsQueryable();
        }

        public async Task<IEnumerable<TEntity>> GetAll()
        {
            return await _db.AsNoTracking().ToListAsync();
        }

        public async Task<IEnumerable<TEntity>> GetAllWithInclude(
            params Expression<Func<TEntity, object>>[] includeProperties)
        {
            return await Include(includeProperties).ToListAsync();
        }

        public async Task<IEnumerable<TEntity>> GetAllWithInclude(Expression<Func<TEntity, bool>> predicate,
            params Expression<Func<TEntity, object>>[] includeProperties)
        {
            var query = Include(includeProperties);
            return await query.Where(predicate).ToListAsync();
        }

        public async Task<IEnumerable<TEntity>> Where(Expression<Func<TEntity, bool>> predicate)
        {
            return await _db.Where(predicate).ToListAsync();
        }

        public async Task<IEnumerable<TEntity>> WhereWithInclude(Expression<Func<TEntity, bool>> predicate, params Expression<Func<TEntity, object>>[] includeProperties)
        {
            var query = Include(includeProperties);
            return await query.Where(predicate).ToListAsync();
        }

        protected IQueryable<TEntity> Include(params Expression<Func<TEntity, object>>[] includeProperties)
        {
            IQueryable<TEntity> query = _db;
            return includeProperties
                .Aggregate(query, (current, includeProperty) => current.Include(includeProperty));
        }
    }
}