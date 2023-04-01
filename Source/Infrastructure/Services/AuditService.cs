using Application.Common.Interfaces;

namespace Infrastructure.Services
{
    public class AuditService<T> : IAuditService<T> where T : class
    {
        public Task AuditAsync(T entity)
        {
            throw new NotImplementedException();
        }
    }
}
