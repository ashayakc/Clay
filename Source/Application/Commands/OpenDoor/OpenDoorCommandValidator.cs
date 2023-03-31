using Application.Common.Interfaces;
using Domain;
using FluentValidation;

namespace Application.Commands.OpenDoor
{
    public class OpenDoorCommandValidator : AbstractValidator<OpenDoorCommand>
    {
        private readonly IRepository<Door> _doorRepository;
        public OpenDoorCommandValidator(IRepository<Door> doorRepository)
        {
            _doorRepository = doorRepository;

            RuleFor(x => x.DoorId)
                .NotEmpty().WithMessage("Invalid Door")
                .MustAsync(IsValidDoorAsync).WithMessage("Invalid door");
        }

        private async Task<bool> IsValidDoorAsync(long doorId, CancellationToken cancellationToken)
        {
            return (await _doorRepository.GetByIdAsync(doorId)) != null;
        }
    }
}
