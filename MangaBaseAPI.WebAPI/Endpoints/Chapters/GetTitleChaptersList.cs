using Carter;
using MangaBaseAPI.Application.Chapters.Queries.GetTitleChaptersList;
using MangaBaseAPI.WebAPI.Common;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace MangaBaseAPI.WebAPI.Endpoints.Chapters
{
    public class GetTitleChaptersList : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet("titles/{id:guid}/chapters-list", HandleGetTitleChapterList)
                .WithTags("Chapters")
                .WithOpenApi(operation => new(operation)
                {
                    Summary = "Get chapters list of a title",
                    Description = "Retrieve chapters list of a title with a given title's ID. The chapters list is a list contains short detail (ID, name, index) of chapters for navigation purposes."
                })
                .MapToApiVersion(1)
                .AllowAnonymous();
        }

        private static async Task<IResult> HandleGetTitleChapterList(
            [FromRoute] Guid id,
            ISender sender,
            CancellationToken cancellationToken)
        {
            var query = new GetTitleChaptersListQuery(id);

            var result = await sender.Send(query, cancellationToken);

            return result.IsSuccess ? Results.Ok(result) : ResultExtensions.HandleFailure(result);
        }
    }
}
