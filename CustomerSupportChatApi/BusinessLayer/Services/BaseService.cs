using CustomerSupportChatApi.BusinessLayer.Interfaces;
using CustomerSupportChatApi.DataLayer.Interfaces;
using System.Linq.Expressions;

namespace CustomerSupportChatApi.BusinessLayer.Services
{
    public class BaseService<TEntity> : IService<TEntity> where TEntity : class
    {
        #region Repository Setup        

        private IRepository<TEntity> repository;
        public BaseService(IRepository<TEntity> repository)
        {
            this.repository = repository;
        }

        #endregion

        public async Task<TEntity> Add(TEntity entity)
        {
            return await repository.Insert(entity);
        }

        public async Task Delete(TEntity entity)
        {
            await repository.Delete(entity);
        }

        public virtual async Task Update(TEntity entity)
        {
            await repository.Update(entity);
        }

        public virtual async Task<IEnumerable<TEntity>> Get(Expression<Func<TEntity, bool>> filter = null, string includeProperties = "")
        {
            return await repository.Get(filter, includeProperties);
        }

        public virtual async Task<IQueryable<TEntity>> GetQueryable(Expression<Func<TEntity, bool>> filter = null, string includeProperties = "")
        {
            return await repository.GetQueryable(filter, includeProperties);
        }

        public void SaveChanges()
        {
            repository.SaveChanges();
        }
    }
}
