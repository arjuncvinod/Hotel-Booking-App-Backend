    using Application.Auth;
    using Hotel.Application.Auth;
    using MediatR;
    using Microsoft.AspNetCore.Mvc;

    namespace Hotel.API.Controllers
    {
        [ApiController]
        [Route("api/auth")]

        public class AuthController : ControllerBase
        {
            private readonly IMediator _mediator;

            public AuthController(IMediator mediator) => _mediator = mediator;

            [HttpPost("login")]
            public async Task<ActionResult<AuthResult>> Login([FromBody] LoginCommand command)
            {

                return Ok(await _mediator.Send(command));

            }


            [HttpPost("refresh")]
            public async Task<ActionResult<AuthResult>> Refresh([FromBody] RefreshCommand command)
            {
                return Ok(await _mediator.Send(command));
            }



        [HttpPost("revoke")]
        public async Task<IActionResult> Revoke([FromBody] RevokeRequest request)
        {
            await _mediator.Send(new RevokeCommand(request.RefreshToken));
            return NoContent();
        }

    }
    }
