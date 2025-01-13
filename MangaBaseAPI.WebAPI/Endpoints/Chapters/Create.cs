using Carter;
using MangaBaseAPI.Application.Chapters.Commands.Create;
using MangaBaseAPI.Contracts.Chapters.Create;
using MangaBaseAPI.Domain.Constants.Authorization;
using MangaBaseAPI.WebAPI.Common;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace MangaBaseAPI.WebAPI.Endpoints.Chapters
{
    public class Create : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPost("titles/{id:guid}/chapters", HandleCreateChapter)
                .WithTags("Chapters")
                .WithOpenApi(operation => new(operation)
                {
                    Summary = "Create new chapter",
                    Description = "Create new chapter for a title with given title ID"
                })
                .MapToApiVersion(1)
                .RequireAuthorization(Policies.AdminRole)
                .DisableAntiforgery();
        }

        private static async Task<IResult> HandleCreateChapter(
            [FromRoute] Guid id,
            [FromForm] CreateChapterRequest request,
            [FromForm] IFormFileCollection chapterImages,
            HttpContext context,
            ISender sender,
            CancellationToken cancellationToken)
        {

            if (!context.Items.TryGetValue("UserId", out var userId))
            {
                return Results.Unauthorized();
            }

            var command = new CreateChapterCommand(
                id,
                request.Name,
                request.Index,
                request.Volume,
                chapterImages,
                Guid.Parse(userId!.ToString()!));

            var result = await sender.Send(command, cancellationToken);

            return result.IsSuccess ? Results.Created(result.Value.Location, result) : ResultExtensions.HandleFailure(result);
        }
    }
}
