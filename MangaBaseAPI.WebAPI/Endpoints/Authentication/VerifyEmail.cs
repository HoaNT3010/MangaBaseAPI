using Carter;
using MangaBaseAPI.Application.Authentication.Commands.VerifyEmail;
using MangaBaseAPI.Contracts.Authentication.VerifyEmail;
using MangaBaseAPI.WebAPI.Common;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace MangaBaseAPI.WebAPI.Endpoints.Authentication
{
    public class VerifyEmail : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPost("auth/email/verify", HandleVerifyEmail)
                .WithTags("Authentication")
                .WithOpenApi(operation => new(operation)
                {
                    Summary = "Verify user account's email address",
                    Description = "Verify email address of user's account via email verification url sent to user."
                })
                .MapToApiVersion(1)
                .AllowAnonymous();
        }

        private static async Task<IResult> HandleVerifyEmail(
            [FromBody] VerifyEmailRequest request,
            ISender sender,
            CancellationToken cancellationToken)
        {
            var command = new VerifyEmailCommand(request.Email, request.Token);

            var result = await sender.Send(command, cancellationToken);

            return result.IsSuccess ? Results.Ok(result) : ResultExtensions.HandleFailure(result);
        }
    }
}
