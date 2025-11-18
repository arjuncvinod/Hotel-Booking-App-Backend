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

        private static readonly CookieOptions RefreshCookieOptions = new()
        {
            HttpOnly = true,
            Secure = true,
            SameSite = SameSiteMode.None,
            Path = "/",
            Expires = DateTimeOffset.UtcNow.AddDays(7)
        };

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginCommand cmd)
        {
            var result = await _mediator.Send(cmd);
            Response.Cookies.Append("refresh_token", result.RefreshToken, RefreshCookieOptions);
            return Ok(new { accessToken = result.AccessToken });
        }

        [HttpPost("refresh")]
        public async Task<IActionResult> Refresh()
        {
            var refreshToken = Request.Cookies["refresh_token"];
            if (string.IsNullOrEmpty(refreshToken))
                return Unauthorized("Missing refresh token");

            var result = await _mediator.Send(new RefreshCommand(refreshToken));
            Response.Cookies.Append("refresh_token", result.RefreshToken, RefreshCookieOptions);
            return Ok(new { accessToken = result.AccessToken });
        }

        [HttpPost("logout")]
        public IActionResult Logout()
        {
            Response.Cookies.Delete("refresh_token", RefreshCookieOptions);
            return Ok();
        }
    }
}