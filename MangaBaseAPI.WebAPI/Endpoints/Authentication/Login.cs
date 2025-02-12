using Carter;
using MangaBaseAPI.Application.Authentication.Commands.Login;
using MangaBaseAPI.Contracts.Authentication.Login;
using MangaBaseAPI.WebAPI.Common;
using MediatR;

namespace MangaBaseAPI.WebAPI.Endpoints.Authentication
{
    public class Login : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPost("auth/login", HandleLogin)
                .WithTags("Authentication")
                .WithOpenApi(operation => new(operation)
                {
                    Summary = "Logs user into the system",
                    Description = "Endpoint for user to log into the system using email and password"
                })
                .MapToApiVersion(1);
        }

        private static async Task<IResult> HandleLogin(
            LoginRequest loginRequest,
            HttpContext context,
            ISender sender,
            CancellationToken cancellationToken)
        {
            var query = new LoginCommand(loginRequest.Email, loginRequest.Password);

            var result = await sender.Send(query, cancellationToken);

            return result.IsSuccess ? Results.Ok(result) : ResultExtensions.HandleFailure(result, context);
        }
    }
}
