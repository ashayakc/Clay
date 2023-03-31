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
}
