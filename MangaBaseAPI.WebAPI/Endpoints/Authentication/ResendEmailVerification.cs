using Carter;
using MangaBaseAPI.Application.Authentication.Commands.ResendEmailVerification;
using MangaBaseAPI.Contracts.Authentication.ResendEmailVerification;
using MangaBaseAPI.WebAPI.Common;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace MangaBaseAPI.WebAPI.Endpoints.Authentication
{
    public class ResendEmailVerification : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPost("auth/email/verify/resend", HandleResendEmailVerification)
                .WithTags("Authentication")
                .WithOpenApi(operation => new(operation)
                {
                    Summary = "Resend email contains user's email verification url",
                    Description = "Resend an email to user's email address containing new email verification url. Old email verification token will be invalidated."
                })
                .MapToApiVersion(1)
                .AllowAnonymous();
        }

        private static async Task<IResult> HandleResendEmailVerification(
            [FromBody] ResendEmailVerificationRequest request,
            ISender sender,
            CancellationToken cancellationToken)
        {
            var command = new ResendEmailVerificationCommand(request.Email);

            var result = await sender.Send(command, cancellationToken);

            return result.IsSuccess ? Results.Ok(result) : ResultExtensions.HandleFailure(result);
        }
    }
}
