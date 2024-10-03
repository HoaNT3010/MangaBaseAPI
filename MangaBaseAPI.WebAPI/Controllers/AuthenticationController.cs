using MangaBaseAPI.Application.Authentication.Queries.Login;
using MangaBaseAPI.Contracts.Authentication.Login;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace MangaBaseAPI.WebAPI.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly ISender _mediator;

        public AuthenticationController(
            ISender mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginRequest request)
        {
            var query = new LoginQuery(request.Email, request.Password);

            var loginResult = await _mediator.Send(query);

            return loginResult == null ? BadRequest() : Ok(loginResult);
        }
    }
}
