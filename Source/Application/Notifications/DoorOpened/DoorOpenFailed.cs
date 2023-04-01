using MediatR;

namespace Application.Notifications.DoorOpened
{
    public class DoorOpenFailed : INotification
    {
        public long DoorId { get; set; }
        public long UserId { get; set; }
        public long OfficeId { get; set; }
        public string Comments { get; set; }
    }

    public class DoorOpenFailedHandler : INotificationHandler<DoorOpenFailed>
    {
        public DoorOpenFailedHandler()
        {

        }

        public Task Handle(DoorOpenFailed notification, CancellationToken cancellationToken)
        {
            //audit here
            return Task.CompletedTask;
        }
    }
}
