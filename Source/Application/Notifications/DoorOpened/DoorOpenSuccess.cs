using MediatR;

namespace Application.Notifications.DoorOpened
{
    public class DoorOpenSuccess : INotification
    {
        public long DoorId { get; set; }
        public long UserId { get; set; }
        public long OfficeId { get; set; }
        public string Comments { get; set; }
    }

    public class DoorOpenSuccessHandler : INotificationHandler<DoorOpenSuccess>
    {
        public DoorOpenSuccessHandler()
        {
            
        }

        public Task Handle(DoorOpenSuccess notification, CancellationToken cancellationToken)
        {
            //audit here
            return Task.CompletedTask;
        }
    }
}
