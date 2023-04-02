using Application.Commands.OpenDoor;
using Application.Common.Interfaces;
using Domain;
using Moq;

namespace Application.Unit.Test.Commands
{
    public class OpenDoorCommandValidatorTests
    {
        private readonly Mock<IGenericRepository<Door>> _doorRepositoryMock;
        private readonly Mock<IGenericRepository<RoleDoorMapping>> _roleDoorRepositoryMock;
        private readonly OpenDoorCommandValidator _validator;

        public OpenDoorCommandValidatorTests()
        {
            _doorRepositoryMock = new Mock<IGenericRepository<Door>>();
            _roleDoorRepositoryMock = new Mock<IGenericRepository<RoleDoorMapping>>();
            _validator = new OpenDoorCommandValidator(_doorRepositoryMock.Object, _roleDoorRepositoryMock.Object);
        }

        [Fact]
        public async Task Given_InvalidDoorId_ShouldReturnValidationError()
        {
            // Arrange
            var command = new OpenDoorCommand
            {
                DoorId = 123,
                RoleId = 1,
                UserId = 1,
                Comments = "Test"
            };

            _doorRepositoryMock.Setup(x => x.GetByIdAsync(command.DoorId))
                               .ReturnsAsync((Door)null);

            // Act
            var result = await _validator.ValidateAsync(command);

            // Assert
            Assert.False(result.IsValid);
            Assert.Contains(result.Errors, error => error.PropertyName == nameof(OpenDoorCommand.DoorId));
        }

        [Fact]
        public async Task Given_InvalidRoleId_ShouldReturnValidationError()
        {
            // Arrange
            var command = new OpenDoorCommand
            {
                DoorId = 1,
                RoleId = 2,
                UserId = 1,
                Comments = "Test"
            };

            _doorRepositoryMock.Setup(x => x.GetByIdAsync(command.DoorId))
                               .ReturnsAsync(new Door());

            _roleDoorRepositoryMock.Setup(x => x.GetAll())
                                   .Returns(new List<RoleDoorMapping>
                                   {
                                   new RoleDoorMapping { RoleId = 1, DoorId = 1 }
                                   }.AsQueryable());

            // Act
            var result = await _validator.ValidateAsync(command);

            // Assert
            Assert.False(result.IsValid);
            Assert.Contains(result.Errors, error => error.PropertyName == nameof(OpenDoorCommand.RoleId));
        }

        [Fact]
        public async Task Given_ValidCommand_ShouldReturnValidationSuccess()
        {
            // Arrange
            var command = new OpenDoorCommand
            {
                DoorId = 1,
                RoleId = 1,
                UserId = 1,
                Comments = "Test"
            };

            _doorRepositoryMock.Setup(x => x.GetByIdAsync(command.DoorId))
                               .ReturnsAsync(new Door());

            _roleDoorRepositoryMock.Setup(x => x.GetAll())
                                   .Returns(new List<RoleDoorMapping>
                                   {
                                   new RoleDoorMapping { RoleId = 1, DoorId = 1 }
                                   }.AsQueryable());

            // Act
            var result = await _validator.ValidateAsync(command);

            // Assert
            Assert.True(result.IsValid);
        }
    }
}
