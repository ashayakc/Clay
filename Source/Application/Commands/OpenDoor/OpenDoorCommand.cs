using Application.Notifications.DoorAction;
using FluentValidation;
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
        private readonly IMediator _mediator;
        private readonly IValidator<OpenDoorCommand> _validator;
        public OpenDoorCommandHandler(IValidator<OpenDoorCommand> validator, IMediator mediator)
        {
            _validator = validator;
            _mediator = mediator;
        }

        public async Task Handle(OpenDoorCommand request, CancellationToken cancellationToken)
        {
            var result = await _validator.ValidateAsync(request, cancellationToken);
            if(!result.IsValid) 
            {
                await _mediator.Publish(new DoorOpenFailed
                {
                    AuditLog = new Domain.Dto.AuditLogDto
                    {
                        DoorId = request.DoorId,
                        UserId = request.UserId,
                        Comments = request.Comments,
                        EventTime = DateTime.UtcNow,
                        Status = 0
                    }
                });
                throw new ValidationException($"Door open failed: {result.Errors[0]}");
            }

            //Validations success. Connect to the harware here and open the door
            await _mediator.Publish(new DoorOpenSuccess 
            {
                AuditLog = new Domain.Dto.AuditLogDto
                {
                    DoorId = request.DoorId,
                    UserId = request.UserId,
                    Comments = request.Comments,
                    EventTime = DateTime.UtcNow,
                    Status = 1
                }
            });
        }
    }
}
