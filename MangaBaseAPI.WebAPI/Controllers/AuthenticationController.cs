using MangaBaseAPI.Application.Authentication.Queries.Login;
using MangaBaseAPI.Contracts.Authentication.Login;
using MangaBaseAPI.WebAPI.Common;
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
        public async Task<IResult> Login(LoginRequest request)
        {
            var query = new LoginQuery(request.Email, request.Password);

            var result = await _mediator.Send(query);

            return result.IsSuccess ? Results.Ok(result) : result.ToProblemDetails();
        }
    }
}
