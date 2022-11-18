using System.Linq.Expressions;

namespace Billing.DAL.Repositories.Interfaces
{
    public interface IGenericRepository<TEntity> where TEntity : class
    {
        Task Add(TEntity item);
        Task<bool> Any(Expression<Func<TEntity, bool>> predicate);
        void Delete(TEntity item);
        Task<TEntity?> Get(long id);
        Task<TEntity?> GetWithInclude(long id, params Expression<Func<TEntity, object>>[] includeProperties);
        Task<IEnumerable<TEntity>> GetAll();
        IQueryable<TEntity> GetAllAsQueryable();
        Task<IEnumerable<TEntity>> GetAllWithInclude(params Expression<Func<TEntity, object>>[] includeProperties);
        Task<IEnumerable<TEntity>> Where(Expression<Func<TEntity, bool>> predicate);
        Task<IEnumerable<TEntity>> WhereWithInclude(Expression<Func<TEntity, bool>> predicate, params Expression<Func<TEntity, object>>[] includeProperties);
        void Update(TEntity item);
    }
}