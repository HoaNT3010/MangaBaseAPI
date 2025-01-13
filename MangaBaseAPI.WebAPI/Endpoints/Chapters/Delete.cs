using Carter;
using MangaBaseAPI.Application.Chapters.Commands.Delete;
using MangaBaseAPI.Domain.Constants.Authorization;
using MangaBaseAPI.WebAPI.Common;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace MangaBaseAPI.WebAPI.Endpoints.Chapters
{
    public class Delete : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapDelete("chapters/{id:guid}", HandleDeleteChapter)
                .WithTags("Chapters")
                .WithOpenApi(operation => new(operation)
                {
                    Summary = "Delete chapter by chapter's ID",
                    Description = "Delete a chapter with a given ID. Chapter will be in soft-deleted state and will be clean up."
                })
                .MapToApiVersion(1)
                .RequireAuthorization(Policies.AdminRole);
        }

        private static async Task<IResult> HandleDeleteChapter(
            [FromRoute] Guid id,
            ISender sender,
            CancellationToken cancellationToken)
        {
            var command = new DeleteChapterCommand(id);

            var result = await sender.Send(command, cancellationToken);

            return result.IsSuccess ? Results.NoContent() : ResultExtensions.HandleFailure(result);
        }
    }
}
