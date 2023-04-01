using Application.Common.Behaviours;
using FluentValidation;
using MediatR;

namespace Application.Unit.Test.Behaviours
{
    public class ValidationBehaviourTest
    {
        [Fact]
        public async Task Handle_ValidRequest_ReturnsResponse()
        {
            // Arrange
            var validators = new List<IValidator<MyRequest>> { new MyRequestValidator() };
            var behaviour = new ValidationBehaviour<MyRequest, MyResponse>(validators);

            var request = new MyRequest
            {
                Age = 20,
                Name = "test",
            };
            var cancellationToken = new CancellationToken();
            RequestHandlerDelegate<MyResponse> next = () => Task.FromResult(new MyResponse());

            // Act
            var response = await behaviour.Handle(request, next, cancellationToken);

            // Assert
            Assert.IsType<MyResponse>(response);
        }

        [Fact]
        public async Task Handle_InvalidRequest_ThrowsValidationException()
        {
            // Arrange
            var validators = new List<IValidator<MyRequest>> { new MyRequestValidator() };
            var behaviour = new ValidationBehaviour<MyRequest, MyResponse>(validators);

            var request = new MyRequest { Name = null };
            var cancellationToken = new CancellationToken();
            RequestHandlerDelegate<MyResponse> next = () => Task.FromResult(new MyResponse());

            // Act
            var exception = await Record.ExceptionAsync(() => behaviour.Handle(request, next, cancellationToken));

            // Assert
            Assert.IsType<ValidationException>(exception);
        }
    }

    public class MyRequest
    {
        public string Name { get; set; }
        public int Age { get; set; }
    }

    public class MyResponse
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
    }

    public class MyRequestValidator : AbstractValidator<MyRequest>
    {
        public MyRequestValidator()
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage("Name is required.");
            RuleFor(x => x.Age).InclusiveBetween(18, 65).WithMessage("Age must be between 18 and 65.");
        }
    }

}
