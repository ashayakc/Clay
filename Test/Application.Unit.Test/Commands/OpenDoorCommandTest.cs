using Application.Commands.OpenDoor;
using Application.Notifications.DoorAction;
using FluentValidation;
using FluentValidation.Results;
using MediatR;
using Moq;

namespace Application.Unit.Test.Commands
{
    public class OpenDoorCommandHandlerTests
    {
        private readonly Mock<IMediator> _mediatorMock;
        private readonly Mock<IValidator<OpenDoorCommand>> _validatorMock;
        private readonly OpenDoorCommandHandler _handler;

        public OpenDoorCommandHandlerTests()
        {
            _mediatorMock = new Mock<IMediator>();
            _validatorMock = new Mock<IValidator<OpenDoorCommand>>();
            _handler = new OpenDoorCommandHandler(_validatorMock.Object, _mediatorMock.Object);
        }

        [Fact]
        public async Task Handle_ValidRequest_PublishesDoorOpenSuccess()
        {
            // Arrange
            var command = new OpenDoorCommand
            {
                DoorId = 1,
                UserId = 1,
                RoleId = 1,
                Comments = "Test"
            };
            var validationResult = new ValidationResult();
            _validatorMock.Setup(v => v.ValidateAsync(command, default)).ReturnsAsync(validationResult);

            // Act
            await _handler.Handle(command, default);

            // Assert
            _mediatorMock.Verify(m => m.Publish(It.Is<DoorOpenSuccess>(d => d.DoorId == command.DoorId && d.UserId == command.UserId && d.Comments == command.Comments), default), Times.Once);
        }

        [Fact]
        public async Task Handle_InvalidRequest_PublishesDoorOpenFailedAndThrowsValidationException()
        {
            // Arrange
            var command = new OpenDoorCommand
            {
                DoorId = 1,
                UserId = 1,
                RoleId = 1,
                Comments = "Test"
            };
            var validationResult = new ValidationResult(new[] { new ValidationFailure("Comments", "Comments is required") });
            _validatorMock.Setup(v => v.ValidateAsync(command, default)).ReturnsAsync(validationResult);

            // Act and Assert
            var ex = await Assert.ThrowsAsync<ValidationException>(() => _handler.Handle(command, default));
            Assert.Equal($"Door open failed: {validationResult.Errors[0]}", ex.Message);

            _mediatorMock.Verify(m => m.Publish(It.Is<DoorOpenFailed>(d => d.DoorId == command.DoorId && d.UserId == command.UserId && d.Comments == command.Comments), default), Times.Once);
        }
    }

}
