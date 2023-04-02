using Application.Common.Interfaces;
using Domain;
using Domain.Dto;
using FluentValidation;
using System.Text;

namespace Application.Commands.Authenticate
{
    public class AuthenticateCommandValidator : AbstractValidator<AuthenticateCommand>
    {
        private readonly IGenericRepository<User> _userRepository;
        public AuthenticateCommandValidator(IGenericRepository<User> userRepository)
        {
            _userRepository = userRepository;

            RuleFor(x => x.Credentials)
                .Must(ValidCredential).WithMessage("Username or password is invalid");
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
                        .FirstOrDefault(x => x.UserName.Equals(username, StringComparison.OrdinalIgnoreCase))!;
        }
    }
}
