using Application.Common.Interfaces;
using Microsoft.EntityFrameworkCore;

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
