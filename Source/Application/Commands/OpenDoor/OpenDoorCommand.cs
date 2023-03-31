using Application.Common.Interfaces;
using Application.Notifications.DoorOpened;
using Domain;
using MediatR;

namespace Application.Commands.OpenDoor
{
    public class OpenDoorCommand : IRequest
    {
        public long DoorId { get; set; }
        public long UserId { get; set; }
        public long RoleId { get; set; }
        public string Comments { get; set; }
    }

    public class OpenDoorCommandHandler : IRequestHandler<OpenDoorCommand>
    {
        private readonly IRepository<Door> _doorRepository;
        private readonly IMediator _mediator;
        public OpenDoorCommandHandler(IRepository<Door> doorRepository, IMediator mediator)
        {
            _doorRepository = doorRepository;
            _mediator = mediator;
        }

        public async Task Handle(OpenDoorCommand request, CancellationToken cancellationToken)
        {
            //add validation to validate if the role has access to this door
            //Connect to the harware here

            await _mediator.Publish(new DoorOpenSuccess 
            {
                DoorId = request.DoorId,
                UserId = request.UserId,
                Comments = request.Comments,
            });
        }
    }
}
