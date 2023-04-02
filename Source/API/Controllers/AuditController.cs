using Application.Queries;
using Domain.Dto;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api")]
    public class AuditController : ControllerBase
    {
        private readonly IMediator _mediator;
        public AuditController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet, Route("users/{userId}/audits")]
        public async Task<IEnumerable<AuditLogDto>> GetByUserIdAsync(long userId, [FromQuery] int from, [FromQuery] int size)
        {
            return await _mediator.Send(new GetAuditQuery
            {
                UserId = userId,
                From = from,
                Size = (size == 0) ? 10 : size
            });
        }
    }
}
