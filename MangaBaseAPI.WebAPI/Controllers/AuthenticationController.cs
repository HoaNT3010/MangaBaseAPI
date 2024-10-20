using MangaBaseAPI.Application.Authentication.Queries.Login;
using MangaBaseAPI.Contracts.Authentication.Login;
using MangaBaseAPI.WebAPI.Common;
using MediatR;
using Microsoft.AspNetCore.Authorization;
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

        /// <summary>
        /// API endpoint user to login into the system with email and password
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login(LoginRequest request)
        {
            var query = new LoginQuery(request.Email, request.Password);

            var result = await _mediator.Send(query);

            return result.IsSuccess ? Ok(result) : this.HandleFailure(result);
        }
    }
}
