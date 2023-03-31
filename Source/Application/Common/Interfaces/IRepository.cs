using Domain;

namespace Application.Common.Interfaces
{
    public interface IRepository<T> where T : class
    {
        IQueryable<T> GetAll();
        ValueTask<T> GetByIdAsync(object id);
    }
}
