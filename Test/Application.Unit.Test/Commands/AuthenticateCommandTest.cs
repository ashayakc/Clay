using Application.Commands.Authenticate;
using Application.Common.Interfaces;
using Domain.Dto;
using Domain;
using FluentValidation;
using Moq;
using FluentValidation.Results;

namespace Application.Unit.Test.Commands
{
    public class AuthenticateCommandHandlerTests
    {
        private readonly Mock<IRepository<User>> _mockUserRepository;
        private readonly Mock<IValidator<AuthenticateCommand>> _mockValidator;
        private readonly AuthenticateCommandHandler _handler;

        public AuthenticateCommandHandlerTests()
        {
            _mockUserRepository = new Mock<IRepository<User>>();
            _mockValidator = new Mock<IValidator<AuthenticateCommand>>();
            _handler = new AuthenticateCommandHandler(_mockUserRepository.Object, _mockValidator.Object);
        }

        [Fact]
        public async Task Handle_ValidRequest_ReturnsJwtToken()
        {
            // Arrange
            var username = "johndoe";
            var request = new AuthenticateCommand { Credentials = new UserCredentialDto { UserName = username } };
            var user = new User { Id = 1, RoleId = 2, IsAdmin = 1, UserName = username };
            _mockUserRepository.Setup(x => x.GetAll()).Returns(new List<User> { user }.AsQueryable());

            // Act
            var result = await _handler.Handle(request, CancellationToken.None);

            // Assert
            Assert.NotEmpty(result);
            Assert.True(result.Length > 0);
        }

        [Fact]
        public void Handle_InvalidRequest_ThrowsValidationException()
        {
            // Arrange
            var request = new AuthenticateCommand { Credentials = new UserCredentialDto { UserName = "" } };
            var validationFailures = new List<ValidationFailure> { new ValidationFailure("UserName", "UserName is required.") };
            _mockValidator.Setup(x => x.ValidateAsync(request, CancellationToken.None)).ReturnsAsync(new ValidationResult(validationFailures));

            // Act & Assert
            Assert.ThrowsAsync<ValidationException>(() => _handler.Handle(request, CancellationToken.None));
        }
    }

}
