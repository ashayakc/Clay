using Application.Common.Interfaces;
using Domain.Dto;
using Nest;

namespace Infrastructure.Services
{
    public class AuditService : IAuditService
    {
        private readonly IElasticClient _elasticClient;
        public AuditService(IElasticClient elasticClient)
        {
            _elasticClient = elasticClient;
        }

        public async Task AuditAsync(AuditLogDto entity)
        {
            await _elasticClient.IndexDocumentAsync(entity);
        }

        public async Task<List<AuditLogDto>> GeAuditAsync(int from, int size, long userId)
        {
            var response = await _elasticClient.SearchAsync<AuditLogDto>(x => x
                .Index("")
                .From(from)
                .Size(size)
                .Query(q => q
                    .Match(t =>
                        t.Field(f => f.UserId)
                        .Query(userId.ToString())
                )));
            return response.Hits.Select(x => x.Source).ToList();
        }
    }
}
