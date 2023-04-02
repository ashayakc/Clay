using Application.Common.Interfaces;
using Application.Constants;
using Domain;
using Domain.Dto;
using FluentValidation;
using MediatR;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Application.Commands.Authenticate
{
    public class AuthenticateCommand : IRequest<string>
    {
        public UserCredentialDto Credentials { get; set; }
    }

    public class AuthenticateCommandHandler : IRequestHandler<AuthenticateCommand, string>
    {
        private readonly IGenericRepository<User> _userRepository;
        private readonly IValidator<AuthenticateCommand> _validator;
        public AuthenticateCommandHandler(IGenericRepository<User> userRepository, IValidator<AuthenticateCommand> validator)
        {
            _userRepository = userRepository;
            _validator = validator;
        }

        public Task<string> Handle(AuthenticateCommand request, CancellationToken cancellationToken)
        {
            return Task.FromResult(GenerateJwtToken(request.Credentials.UserName));
        }

        private string GenerateJwtToken(string userName)
        {
            // generate token that is valid for 7 days
            var tokenHandler = new JwtSecurityTokenHandler();
            var user = GetUser(userName);
            var key = Encoding.ASCII.GetBytes("THIS IS USED TO SIGN AND VERIFY JWT TOKENS, REPLACE IT WITH YOUR OWN SECRET, IT CAN BE ANY STRING");
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(Jwt.ID, user.Id.ToString()),
                    new Claim(Jwt.ROLEID, user.RoleId.ToString()),
                    new Claim(Jwt.IS_ADMIN, user.IsAdmin.ToString())
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
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
