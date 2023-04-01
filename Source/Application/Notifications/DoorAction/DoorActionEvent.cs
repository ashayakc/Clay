using Application.Common.Interfaces;
using Domain.Dto;
using MediatR;

namespace Application.Notifications.DoorAction
{
    public class DoorOpenSuccess : INotification
    {
        public AuditLogDto AuditLog { get; set; }
    }

    public class DoorOpenFailed : INotification
    {
        public AuditLogDto AuditLog { get; set; }
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
            return _auditService.AuditAsync(notification.AuditLog);
        }

        public Task Handle(DoorOpenSuccess notification, CancellationToken cancellationToken)
        {
            return _auditService.AuditAsync(notification.AuditLog);
        }
    }
}
