using Domain.Dto;

namespace Application.Common.Interfaces
{
    public interface IAuditService
    {
        Task AuditAsync(AuditLogDto entity);
        Task<List<AuditLogDto>> GeAuditAsync(int from, int size, long userId);
    }
}
