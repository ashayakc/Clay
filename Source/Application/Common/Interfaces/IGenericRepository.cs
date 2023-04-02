using System.Linq.Expressions;

namespace Application.Common.Interfaces
{
    public interface IGenericRepository<T> where T : class
    {
        IQueryable<T> GetAll();
        Task<IQueryable<T>> GetAsync(Expression<Func<T, bool>> filter,
            string includeProperties);
        ValueTask<T> GetByIdAsync(object id);
    }
}
