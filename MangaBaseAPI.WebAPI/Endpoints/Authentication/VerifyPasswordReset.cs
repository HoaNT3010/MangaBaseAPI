using Carter;
using MangaBaseAPI.Application.Authentication.Commands.VerifyPasswordReset;
using MangaBaseAPI.Contracts.Authentication.VerifyPasswordReset;
using MangaBaseAPI.WebAPI.Common;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace MangaBaseAPI.WebAPI.Endpoints.Authentication
{
    public class VerifyPasswordReset : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPost("auth/password/reset", HandleVerifyPasswordReset)
                .WithTags("Authentication")
                .WithOpenApi(operation => new(operation)
                {
                    Summary = "Verify user password reset operation",
                    Description = "Verify user's password reset operation via password reset url sent to user's email address."
                })
                .MapToApiVersion(1)
                .AllowAnonymous();
        }

        private static async Task<IResult> HandleVerifyPasswordReset(
            [FromBody] VerifyPasswordResetRequest request,
            ISender sender,
            CancellationToken cancellationToken)
        {
            var command = new VerifyPasswordResetCommand(
                request.Email,
                request.NewPassword,
                request.ConfirmNewPassword,
                request.Token);

            var result = await sender.Send(command, cancellationToken);

            return result.IsSuccess ? Results.Ok(result) : ResultExtensions.HandleFailure(result);
        }
    }
}
