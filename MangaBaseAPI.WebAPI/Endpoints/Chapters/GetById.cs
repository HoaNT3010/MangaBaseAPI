using Carter;
using MangaBaseAPI.Application.Chapters.Queries.GetById;
using MangaBaseAPI.WebAPI.Common;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace MangaBaseAPI.WebAPI.Endpoints.Chapters
{
    public class GetById : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet("chapters/{id:guid}", HandleGetChapterById)
                .WithTags("Chapters")
                .WithOpenApi(operation => new(operation)
                {
                    Summary = "Get chapter by chapter's ID",
                    Description = "Retrieve information of a chapter with a given ID"
                })
                .MapToApiVersion(1)
                .AllowAnonymous();
        }

        private static async Task<IResult> HandleGetChapterById(
            [FromRoute] Guid id,
            HttpContext context,
            ISender sender,
            CancellationToken cancellationToken)
        {

            var query = new GetChapterByIdQuery(id);

            var result = await sender.Send(query, cancellationToken);

            return result.IsSuccess ? Results.Ok(result) : ResultExtensions.HandleFailure(result, context);
        }
    }
}
