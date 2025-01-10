using Carter;
using MangaBaseAPI.Application.Chapters.Queries.GetChaptersByTitleId;
using MangaBaseAPI.Contracts.Chapters.GetChaptersByTitleId;
using MangaBaseAPI.WebAPI.Common;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace MangaBaseAPI.WebAPI.Endpoints.Chapters
{
    public class GetChaptersByTitleId : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet("titles/{id:guid}/chapters", HandleGetChaptersByTitleId)
                .WithTags("Chapters")
                .WithOpenApi(operation => new(operation)
                {
                    Summary = "Get chapters of a title",
                    Description = "Retrieve chapters list of a title with a given title's ID. Paging and sort by index order can be applied."
                })
                .MapToApiVersion(1)
                .AllowAnonymous();
        }

        private static async Task<IResult> HandleGetChaptersByTitleId(
            [FromRoute] Guid id,
            GetChaptersByTitleIdRequest request,
            ISender sender,
            CancellationToken cancellationToken)
        {
            var query = new GetChaptersByTitleIdQuery(id,
                request.Page,
                request.PageSize,
                request.DescendingIndexOrder);

            var result = await sender.Send(query, cancellationToken);

            return result.IsSuccess ? Results.Ok(result) : ResultExtensions.HandleFailure(result);
        }
    }
}
