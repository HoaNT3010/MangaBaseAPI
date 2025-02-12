using Carter;
using MangaBaseAPI.Application.Authentication.Commands.SendPasswordResetEmail;
using MangaBaseAPI.Contracts.Authentication.SendPasswordResetEmail;
using MangaBaseAPI.WebAPI.Common;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace MangaBaseAPI.WebAPI.Endpoints.Authentication
{
    public class SendPasswordResetEmail : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPost("auth/password/reset/send", HandleSendPasswordResetEmail)
                .WithTags("Authentication")
                .WithOpenApi(operation => new(operation)
                {
                    Summary = "Send email contains password reset url for user's account",
                    Description = "Send an email to user's email address containing password reset url. Old password reset token will be invalidated."
                })
                .MapToApiVersion(1)
                .AllowAnonymous();
        }

        private static async Task<IResult> HandleSendPasswordResetEmail(
            [FromBody] SendPasswordResetEmailRequest request,
            HttpContext context,
            ISender sender,
            CancellationToken cancellationToken)
        {
            var command = new SendPasswordResetEmailCommand(request.Email);

            var result = await sender.Send(command, cancellationToken);

            return result.IsSuccess ? Results.Ok(result) : ResultExtensions.HandleFailure(result, context);
        }
    }
}
