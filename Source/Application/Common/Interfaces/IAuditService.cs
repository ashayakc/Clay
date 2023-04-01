namespace Application.Common.Interfaces
{
    public interface IAuditService<T> where T : class
    {
        Task AuditAsync(T entity);
    }
}
