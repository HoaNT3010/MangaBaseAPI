using Carter;
using MangaBaseAPI.Application.Users.Commands.UpdateFullName;
using MangaBaseAPI.Contracts.Users.UpdateFullName;
using MangaBaseAPI.WebAPI.Common;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace MangaBaseAPI.WebAPI.Endpoints.Users
{
    public class UpdateFullName : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPut("users/fullname", HandleUpdateUserFullName)
                .WithTags("Users")
                .WithOpenApi(operation => new(operation)
                {
                    Summary = "Update user's full name",
                    Description = "Update user's full name with user provided first and last names."
                })
                .MapToApiVersion(1)
                .RequireAuthorization();
        }

        private static async Task<IResult> HandleUpdateUserFullName(
            [FromBody] UpdateUserFullNameRequest request,
            HttpContext context,
            ISender sender,
            CancellationToken cancellationToken)
        {
            if (!context.Items.TryGetValue("UserId", out var userId))
            {
                return Results.Unauthorized();
            }

            var command = new UpdateUserFullNameCommand(
                Guid.Parse(userId!.ToString()!),
                    request.FirstName,
                    request.LastName);

            var result = await sender.Send(command, cancellationToken);

            return result.IsSuccess ? Results.NoContent() : ResultExtensions.HandleFailure(result, context);
        }
    }
}
