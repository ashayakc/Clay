using API.Authorization;
using Microsoft.AspNetCore.Mvc;
using Domain.Dto;
using MediatR;
using Application.Commands.OpenDoor;

namespace API.Controllers
{
    [ApiController]
    [Route("api/doors")]
    [ApiVersion("1")]
    public class DoorsController : ControllerBase
    {
        private readonly IClaimsHandler _claimsHandler;
        private readonly IMediator _mediator;
        public DoorsController(IClaimsHandler claimsHandler, IMediator mediator)
        {
            _claimsHandler = claimsHandler;
            _mediator = mediator;
        }

        //[HttpGet]
        //public async Task<IEnumerable<DoorDto>> GetAsync()
        //{
        //    return await _doorService.GetByUserIdAsync(_claimsHandler.GetUserId(User.Claims));
        //}

        [HttpPost, Route("{doorId}/open")]
        public async Task<IActionResult> OpenAsync(long doorId, [FromBody] string comments)
        {
            await _mediator.Send(new OpenDoorCommand
            {
                DoorId = doorId, 
                UserId = _claimsHandler.GetUserId(User.Claims),
                RoleId = _claimsHandler.GetRoleId(User.Claims),
                Comments = comments
            });
            return Ok("Door opened successfully");
        }
    }
}