using Carter;
using MangaBaseAPI.Application.Authentication.Commands.RefreshToken;
using MangaBaseAPI.Contracts.Authentication.RefreshToken;
using MangaBaseAPI.WebAPI.Common;
using MediatR;

namespace MangaBaseAPI.WebAPI.Endpoints.Authentication
{
    public class RefreshToken : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPost("auth/refresh-token", HandleRefreshToken)
                .WithTags("Authentication")
                .WithOpenApi(operation => new(operation)
                {
                    Summary = "Provide JWT refresh and access tokens to user",
                    Description = "Endpoint for user to retrieve new jwt refresh and access token with a valid refresh token"
                })
                .MapToApiVersion(1);
        }

        private static async Task<IResult> HandleRefreshToken(
            RefreshTokenRequest refreshTokenRequest,
            HttpContext context,
            ISender sender,
            CancellationToken cancellationToken)
        {
            var command = new RefreshTokenCommand(refreshTokenRequest.refreshToken);

            var result = await sender.Send(command, cancellationToken);

            return result.IsSuccess ? Results.Ok(result) : ResultExtensions.HandleFailure(result, context);
        }
    }
}
