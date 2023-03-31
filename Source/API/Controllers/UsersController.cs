using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Domain.Dto;
using Swashbuckle.AspNetCore.Annotations;
using MediatR;
using Application.Commands.Authenticate;

namespace API.Controllers
{
    [ApiController]
    [Route("api/users")]
    [ApiVersion("1")]
    public class UsersController : ControllerBase
    {
        private readonly IMediator _mediator;
        public UsersController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost, Route("login")]
        [AllowAnonymous]
        [SwaggerOperation(OperationId = "login")]
        public async Task<IActionResult> AuthenticateAsync([FromBody] UserCredentialDto userCredential)
        {
            return Ok(await _mediator.Send(new AuthenticateCommand
            {
                Credentials = userCredential
            }));
        }
    }
}
