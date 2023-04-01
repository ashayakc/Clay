using Application.Commands.Authenticate;
using Application.Common.Interfaces;
using Domain;
using Domain.Dto;
using Moq;
using System.Text;

namespace Application.Unit.Test.Commands
{
    public class AuthenticateCommandValidatorTests
    {
        private readonly Mock<IRepository<User>> _userRepositoryMock;
        private readonly AuthenticateCommandValidator _validator;

        public AuthenticateCommandValidatorTests()
        {
            _userRepositoryMock = new Mock<IRepository<User>>();
            _validator = new AuthenticateCommandValidator(_userRepositoryMock.Object);
        }

        [Fact]
        public void ValidCredential_ShouldReturnTrue()
        {
            // Arrange
            var userCredential = new UserCredentialDto
            {
                UserName = "john.doe",
                Password = "password"
            };
            var user = new User
            {
                UserName = "john.doe",
                Password = Convert.ToBase64String(Encoding.UTF8.GetBytes("password"))
            };
            _userRepositoryMock.Setup(x => x.GetAll()).Returns(new[] { user }.AsQueryable());

            // Act
            var result = _validator.Validate(new AuthenticateCommand { Credentials = userCredential });

            // Assert
            Assert.True(result.IsValid);
        }

        [Fact]
        public void InvalidCredential_ShouldReturnFalse()
        {
            // Arrange
            var userCredential = new UserCredentialDto
            {
                UserName = "john.doe",
                Password = "wrong_password"
            };
            var user = new User
            {
                UserName = "john.doe",
                Password = Convert.ToBase64String(Encoding.UTF8.GetBytes("password"))
            };
            _userRepositoryMock.Setup(x => x.GetAll()).Returns(new[] { user }.AsQueryable());

            // Act
            var result = _validator.Validate(new AuthenticateCommand { Credentials = userCredential });

            // Assert
            Assert.False(result.IsValid);
            Assert.Equal("Username or password is invalid", result.Errors.Single().ErrorMessage);
        }
    }
}
