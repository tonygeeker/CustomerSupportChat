using CustomerSupportChatApi.DataLayer.Contexts;
using CustomerSupportChatApi.DataLayer.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Immutable;
using System.Linq.Expressions;

namespace CustomerSupportChatApi.DataLayer.Repositories
{
    public class GenericRepository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        internal CustomerSupportContext _context;
        internal DbSet<TEntity> _dbSet;

        public GenericRepository(CustomerSupportContext context)
        {
            _context = context;
            _dbSet = context.Set<TEntity>();
        }

        /// <summary>
        /// Buffering 
        /// Returns a set of entities which match the given filter, optional eager loading
        /// </summary>
        /// <param name="filter">Lambda expression determining the filter to be applied on the entities (ex. student => student.LastName == "Smith")</param>
        /// <param name="orderBy">Lambda expression declaring the orderBy expression (ex. q => q.OrderBy(s => s.LastName))</param>
        /// <param name="includeProperties">Comma-delimited list of navigation properties for eager loading</param>
        /// <returns></returns>
        public virtual async Task<IEnumerable<TEntity>> Get(Expression<Func<TEntity, bool>> filter = null, string includeProperties = "")
        {
            IQueryable<TEntity> query = _dbSet;

            if (filter != null)
            {
                query = query.Where(filter);
            }

            if (!string.IsNullOrEmpty(includeProperties))
            {
                foreach (var includeProperty in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(includeProperty);
                }
            }

            return query.ToList();
        }

        /// <summary>
        /// Streaming
        /// Returns a Queryable of entities which match the given filter, optional eager loading
        /// </summary>
        /// <param name="filter">Lambda expression determining the filter to be applied on the entities (ex. student => student.LastName == "Smith")</param>
        /// <param name="orderBy">Lambda expression declaring the orderBy expression (ex. q => q.OrderBy(s => s.LastName))</param>
        /// <param name="includeProperties">Comma-delimited list of navigation properties for eager loading</param>
        /// <returns></returns>
        public virtual async Task<IQueryable<TEntity>> GetQueryable(Expression<Func<TEntity, bool>> filter = null, string includeProperties = "")
        {
            IQueryable<TEntity> query = _dbSet;

            if (filter != null)
            {
                query = query.Where(filter);
            }

            if (!string.IsNullOrEmpty(includeProperties))
            {
                foreach (var includeProperty in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(includeProperty);
                }
            }

            return query;
        }

        public virtual async Task<TEntity> Insert(TEntity entity)
        {
            var currentDate = DateTime.Now;

            return (await _dbSet.AddAsync(entity)).Entity;
        }

        public async Task Update(TEntity entity)
        {
            _dbSet.Attach(entity);
            var entry = _context.Entry(entity);
            entry.State = EntityState.Modified;
        }

        public virtual async Task Delete(TEntity entityToDelete)
        {
            if (_context.Entry(entityToDelete).State == EntityState.Detached)
            {
                _dbSet.Attach(entityToDelete);
            }
            _dbSet.Remove(entityToDelete);
        }

        public virtual void SaveChanges()
        {
            try
            {
                _context.SaveChanges();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                ex.Entries.Single().Reload();
                SaveChanges();
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        #region IDisposable Members  

        private bool disposed = false;
        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
            }
            disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion

    }
}
