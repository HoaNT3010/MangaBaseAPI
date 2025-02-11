using Carter;
using MangaBaseAPI.Application.Users.Commands.ChangePassword;
using MangaBaseAPI.Contracts.Users.ChangePassword;
using MangaBaseAPI.WebAPI.Common;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace MangaBaseAPI.WebAPI.Endpoints.Users
{
    public class ChangePassword : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPut("users/password", HandleChangePassword)
                .WithTags("Users")
                .WithOpenApi(operation => new(operation)
                {
                    Summary = "Update user's account password",
                    Description = "Update user's account password with provided information."
                })
                .MapToApiVersion(1)
                .RequireAuthorization();
        }

        private static async Task<IResult> HandleChangePassword(
            [FromBody] ChangePasswordRequest request,
            HttpContext context,
            ISender sender,
            CancellationToken cancellationToken)
        {
            if (!context.Items.TryGetValue("UserId", out var userId))
            {
                return Results.Unauthorized();
            }
            if (string.IsNullOrWhiteSpace(userId!.ToString()) || !Guid.TryParse(userId.ToString(), out Guid userGuid))
            {
                return Results.Unauthorized();
            }

            var command = new ChangePasswordCommand(
                request.CurrentPassword,
                request.NewPassword,
                request.ConfirmNewPassword,
                userGuid);

            var result = await sender.Send(command, cancellationToken);

            return result.IsSuccess ? Results.NoContent() : ResultExtensions.HandleFailure(result);
        }
    }
}
