using Application.Common.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Infrastructure.Persistance
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly LockDbContext _dbContext;
        private DbSet<T> _dbSet;
        public Repository(LockDbContext dbContext)
        {
            _dbContext = dbContext;
            _dbSet = _dbContext.Set<T>();
        }

        public async Task<IQueryable<T>> GetAsync(Expression<Func<T, bool>> filter, string includeProperties)
        {
            IQueryable<T> query = _dbSet;

            if (filter != null)
            {
                query = query.Where(filter);
            }

            if (!string.IsNullOrEmpty(includeProperties))
            {
                foreach (var includeProperty in includeProperties.Split
                (new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(includeProperty);
                }
            }

            return query;
        }

        public IQueryable<T> GetAll()
        {
            return _dbSet;
        }

        public ValueTask<T> GetByIdAsync(object id)
        {
            return _dbSet.FindAsync(id);
        }
    }
}
