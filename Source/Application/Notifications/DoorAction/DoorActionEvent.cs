using Application.Common.Interfaces;
using Domain.Dto;
using MediatR;

namespace Application.Notifications.DoorAction
{
    public class DoorOpenSuccess : INotification
    {
        public long UserId { get; set; }
        public long DoorId { get; set; }
        public long OfficeId { get; set; }
        public string? Comments { get; set; }
    }

    public class DoorOpenFailed : INotification
    {
        public long UserId { get; set; }
        public long DoorId { get; set; }
        public long OfficeId { get; set; }
        public string? Comments { get; set; }
    }

    public class DoorActionHandler : INotificationHandler<DoorOpenFailed>,
        INotificationHandler<DoorOpenSuccess>
    {
        private readonly IAuditService<AuditLogDto> _auditService;
        public DoorActionHandler(IAuditService<AuditLogDto> auditService)
        {
            _auditService = auditService;
        }

        public Task Handle(DoorOpenFailed notification, CancellationToken cancellationToken)
        {
            return _auditService.AuditAsync(new AuditLogDto
            {
                DoorId = notification.DoorId,
                UserId = notification.UserId,
                Comments = notification.Comments,
                OfficeId = notification.OfficeId,
                Status = 0,
                EventTime = DateTime.UtcNow
            });
        }

        public Task Handle(DoorOpenSuccess notification, CancellationToken cancellationToken)
        {
            return _auditService.AuditAsync(new AuditLogDto
            {
                DoorId = notification.DoorId,
                UserId = notification.UserId,
                Comments = notification.Comments,
                OfficeId = notification.OfficeId,
                Status = 1,
                EventTime = DateTime.UtcNow
            });
        }
    }
}
