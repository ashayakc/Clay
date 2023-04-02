using Application.Common.Interfaces;
using Domain.Dto;
using MediatR;

namespace Application.Queries
{
    public record GetAuditQuery : IRequest<List<AuditLogDto>>
    {
        public long UserId { get; set; }
        public int From { get; set; }
        public int Size { get; set; }
    }

    public class GetAuditQueryHandler : IRequestHandler<GetAuditQuery, List<AuditLogDto>>
    {
        private readonly IAuditService _auditService;
        public GetAuditQueryHandler(IAuditService auditService)
        {
            _auditService = auditService;
        }

        public Task<List<AuditLogDto>> Handle(GetAuditQuery request, CancellationToken cancellationToken)
        {
            return (_auditService.GeAuditAsync(request.From, request.Size, request.UserId));
        }
    }
}
