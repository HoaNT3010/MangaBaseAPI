using Carter;
using MangaBaseAPI.Application.Authentication.Commands.RevokeToken;
using MangaBaseAPI.Contracts.Authentication.RevokeToken;
using MangaBaseAPI.WebAPI.Common;
using MediatR;

namespace MangaBaseAPI.WebAPI.Endpoints.Authentication
{
    public class RevokeToken : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPost("auth/revoke-token", HandleRevokeToken)
                .WithTags("Authentication")
                .WithOpenApi(operation => new(operation)
                {
                    Summary = "Revoke user's refresh token",
                    Description = "Endpoint for user to revoke their refresh token"
                })
                .MapToApiVersion(1);
        }

        private static async Task<IResult> HandleRevokeToken(
            RevokeTokenRequest revokeTokenRequest,
            HttpContext context,
            ISender sender,
            CancellationToken cancellationToken)
        {
            var command = new RevokeTokenCommand(revokeTokenRequest.refreshToken);

            var result = await sender.Send(command, cancellationToken);

            return result.IsSuccess ? Results.Ok(result) : ResultExtensions.HandleFailure(result, context);
        }
    }
}
