using System.Linq.Expressions;

namespace CustomerSupportChatApi.DataLayer.Interfaces
{
    public interface IRepository<TEntity> : IDisposable
    {
        Task<IEnumerable<TEntity>> Get(Expression<Func<TEntity, bool>> filter = null, string includeProperties = "");

        Task<IQueryable<TEntity>> GetQueryable(Expression<Func<TEntity, bool>> filter = null, string includeProperties = "");

        Task<TEntity> Insert(TEntity entity);

        Task Delete(TEntity entityToDelete);
        Task Update(TEntity entity);

        void SaveChanges();
    }
}
