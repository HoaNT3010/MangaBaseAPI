using Carter;
using MangaBaseAPI.Contracts.Users.UpdateUserName;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using MangaBaseAPI.WebAPI.Common;
using MangaBaseAPI.Application.Users.Commands.UpdateUserName;

namespace MangaBaseAPI.WebAPI.Endpoints.Users
{
    public class UpdateUserName : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPut("users/username", HandleUpdateUserUserName)
                .WithTags("Users")
                .WithOpenApi(operation => new(operation)
                {
                    Summary = "Update user's username",
                    Description = "Update user's username. The new username is compared ignore case with other usernames for uniqueness."
                })
                .MapToApiVersion(1)
                .RequireAuthorization();
        }

        private static async Task<IResult> HandleUpdateUserUserName(
            [FromBody] UpdateUserUserNameRequest request,
            HttpContext context,
            ISender sender,
            CancellationToken cancellationToken)
        {
            if (!context.Items.TryGetValue("UserId", out var userId))
            {
                return Results.Unauthorized();
            }

            var command = new UpdateUserUserNameCommand(
                Guid.Parse(userId!.ToString()!),
                request.UserName);

            var result = await sender.Send(command, cancellationToken);

            return result.IsSuccess ? Results.NoContent() : ResultExtensions.HandleFailure(result, context);
        }
    }
}
