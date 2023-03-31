using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.JsonWebTokens;
using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Text.RegularExpressions;

namespace API.Authorization
{
    public static class ServiceCollectionExtensions
    {
        public static void AddJwtAuthentication(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtAuthenticationHandler.AuthScheme;
                options.DefaultChallengeScheme = JwtAuthenticationHandler.AuthScheme;
            }).AddScheme<AuthenticationSchemeOptions, JwtAuthenticationHandler>(JwtAuthenticationHandler.AuthScheme, _ => { });
        }

        private class JwtAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
        {
            public const string AuthScheme = "JwtAuth";

            public JwtAuthenticationHandler(IOptionsMonitor<AuthenticationSchemeOptions> options,
                                             ILoggerFactory logger,
                                             UrlEncoder encoder,
                                             ISystemClock clock) : base(options, logger, encoder, clock)
            {
            }

            protected override Task<AuthenticateResult> HandleAuthenticateAsync()
            {
                if (!Request.Headers.ContainsKey("Authorization"))
                {
                    return Task.FromResult(AuthenticateResult.Fail("Unauthorized"));
                }

                string authorization = Request.Headers["Authorization"];
                if (string.IsNullOrEmpty(authorization))
                {
                    return Task.FromResult(AuthenticateResult.Fail("Unauthorized"));
                }

                if (!authorization.StartsWith("bearer", StringComparison.OrdinalIgnoreCase))
                {
                    return Task.FromResult(AuthenticateResult.Fail("Unauthorized"));
                }

                string token = Regex.Replace(authorization, "bearer ", "", RegexOptions.IgnoreCase);

                if (string.IsNullOrEmpty(token))
                {
                    return Task.FromResult(AuthenticateResult.Fail("Unauthorized"));
                }

                try
                {
                    return ReadToken(token);
                }
                catch (Exception ex)
                {
                    return Task.FromResult(AuthenticateResult.Fail(ex.Message));
                }
            }

            private Task<AuthenticateResult> ReadToken(string token)
            {
                var jsonWebToken = new JsonWebToken(token);
                var authenticationTicket = new AuthenticationTicket(
                    new ClaimsPrincipal(new ClaimsIdentity(jsonWebToken.Claims, AuthScheme)),
                    new AuthenticationProperties(),
                    AuthScheme);
                return Task.FromResult(AuthenticateResult.Success(authenticationTicket));
            }
        }
    }
}
