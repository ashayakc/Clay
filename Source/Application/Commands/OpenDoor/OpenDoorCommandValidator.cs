using Application.Common.Interfaces;
using Domain;
using FluentValidation;

namespace Application.Commands.OpenDoor
{
    public class OpenDoorCommandValidator : AbstractValidator<OpenDoorCommand>
    {
        private readonly IRepository<Door> _doorRepository;
        private readonly IRepository<RoleDoorMapping> _roleDoorRepository;
        public OpenDoorCommandValidator(IRepository<Door> doorRepository, IRepository<RoleDoorMapping> roleDoorRepository)
        {
            _doorRepository = doorRepository;
            _roleDoorRepository = roleDoorRepository;

            RuleFor(x => x.DoorId)
                .NotEmpty().WithMessage("Invalid Door")
                .MustAsync(IsValidDoorAsync).WithMessage("Invalid door");

            RuleFor(user => user.RoleId)
                .NotEmpty().WithMessage("Invalid Role")
                .Must((user, roleId) => IsValidRole(user)).WithMessage("You do not have access to this door");
        }

        private async Task<bool> IsValidDoorAsync(long doorId, CancellationToken cancellationToken)
        {
            return (await _doorRepository.GetByIdAsync(doorId)) != null;
        }

        private bool IsValidRole(OpenDoorCommand user)
        {
            return (_roleDoorRepository
                .GetAll().AsEnumerable()
                .FirstOrDefault(x => x.RoleId == user.RoleId && x.DoorId == user.DoorId)) != null;
        }
    }
}
