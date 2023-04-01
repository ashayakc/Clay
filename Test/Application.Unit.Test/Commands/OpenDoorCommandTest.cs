using Application.Commands.OpenDoor;
using Application.Notifications.DoorOpened;
using FluentValidation;
using FluentValidation.Results;
using MediatR;
using Moq;

namespace Application.Unit.Test.Commands
{
    public class OpenDoorCommandHandlerTests
    {
        private Mock<IMediator> _mediatorMock;
        private Mock<IValidator<OpenDoorCommand>> _validatorMock;
        private OpenDoorCommandHandler _handler;

        public OpenDoorCommandHandlerTests()
        {
            _mediatorMock = new Mock<IMediator>();
            _validatorMock = new Mock<IValidator<OpenDoorCommand>>();
            _handler = new OpenDoorCommandHandler(_validatorMock.Object, _mediatorMock.Object);
        }

        [Fact]
        public async Task Handle_WhenValidationSucceeds_PublishesDoorOpenSuccess()
        {
            // Arrange
            var request = new OpenDoorCommand
            {
                DoorId = 1,
                UserId = 2,
                RoleId = 3,
                Comments = "Test comment"
            };

            _validatorMock.Setup(v => v.ValidateAsync(request, default))
                .ReturnsAsync(new ValidationResult());

            // Act
            await _handler.Handle(request, default);

            // Assert
            _mediatorMock.Verify(m => m.Publish(
                It.Is<DoorOpenSuccess>(d => d.DoorId == request.DoorId && d.UserId == request.UserId && d.Comments == request.Comments),
                default), Times.Once);

            _mediatorMock.Verify(m => m.Publish(
                It.IsAny<DoorAction>(), default), Times.Never);
        }

        [Fact]
        public async Task Handle_WhenValidationFails_PublishesDoorOpenFailedAndThrowsValidationException()
        {
            // Arrange
            var request = new OpenDoorCommand
            {
                DoorId = 1,
                UserId = 2,
                RoleId = 3,
                Comments = "Test comment"
            };

            var validationErrors = new List<ValidationFailure>
            {
                new ValidationFailure("DoorId", "Door ID is invalid")
            };

            _validatorMock.Setup(v => v.ValidateAsync(request, default))
                .ReturnsAsync(new ValidationResult(validationErrors));

            // Act & Assert
            await Assert.ThrowsAsync<ValidationException>(() => _handler.Handle(request, default));

            _mediatorMock.Verify(m => m.Publish(
                It.Is<DoorAction>(d => d.DoorId == request.DoorId && d.UserId == request.UserId && d.Comments == request.Comments),
                default), Times.Once);

            _mediatorMock.Verify(m => m.Publish(
                It.IsAny<DoorOpenSuccess>(), default), Times.Never);
        }
    }
}
