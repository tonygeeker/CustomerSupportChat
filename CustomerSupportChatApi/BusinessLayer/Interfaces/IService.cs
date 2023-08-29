using System.Linq.Expressions;

namespace CustomerSupportChatApi.BusinessLayer.Interfaces
{
    public interface IService<TEntity>
    {
        Task<TEntity> Add(TEntity entity);
        Task Delete(TEntity entity);
        Task Update(TEntity entity);
        Task<IEnumerable<TEntity>> Get(Expression<Func<TEntity, bool>> filter = null, string includeProperties = "");
        Task<IQueryable<TEntity>> GetQueryable(Expression<Func<TEntity, bool>> filter = null, string includeProperties = "");
        void SaveChanges();
    }
}
