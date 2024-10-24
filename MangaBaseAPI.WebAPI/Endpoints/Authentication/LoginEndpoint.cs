using Carter;
using MangaBaseAPI.Application.Authentication.Queries.Login;
using MangaBaseAPI.Contracts.Authentication.Login;
using MangaBaseAPI.WebAPI.Common;
using MediatR;

namespace MangaBaseAPI.WebAPI.Endpoints.Authentication
{
    public class LoginEndpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPost("api/auth/login", Login)
                .WithTags("Authentication")
                .WithOpenApi(op => new(op)
                {
                    Summary = "Logs user into the system",
                    Description = "Endpoint for user to log into the system using email and password"
                });
        }

        public static async Task<IResult> Login(
            LoginRequest loginRequest,
            ISender sender)
        {
            var query = new LoginQuery(loginRequest.Email, loginRequest.Password);

            var result = await sender.Send(query);

            return result.IsSuccess ? Results.Ok(result) : ResultExtensions.HandleFailure(result);
        }
    }
}
