using Application.Common.Interfaces;
using Domain;
using Domain.Dto;
using FluentValidation;
using System.Text;

namespace Application.Commands.Authenticate
{
    public class AuthenticateCommandValidator : AbstractValidator<AuthenticateCommand>
    {
        private readonly IRepository<User> _userRepository;
        public AuthenticateCommandValidator(IRepository<User> userRepository)
        {
            _userRepository = userRepository;

            RuleFor(x => x.Credentials)
                .Must(ValidCredential).WithMessage("Username or password is invalid");

            RuleFor(x => x.Credentials.Password)
                .MinimumLength(3).WithMessage("Password should be of atleast 3 characters");
        }

        private bool ValidCredential(UserCredentialDto userCredential)
        {
            var user = GetUser(userCredential.UserName);
            var encodedPassword = Convert.ToBase64String(Encoding.UTF8.GetBytes(userCredential.Password));
            if (user == null || user.Password != encodedPassword)
                return false;
            return true;
        }

        private User GetUser(string username)
        {
            return _userRepository
                        .GetAll()
                        .AsEnumerable()
                        .First(x => x.UserName.Equals(username, StringComparison.OrdinalIgnoreCase));
        }
    }
}
