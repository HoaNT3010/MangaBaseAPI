using Carter;
using MangaBaseAPI.Application.Titles.Queries.GetRecentlyUpdatedTitles;
using MangaBaseAPI.Contracts.Titles.GetRecentlyUpdatedTitles;
using MangaBaseAPI.WebAPI.Common;
using MediatR;

namespace MangaBaseAPI.WebAPI.Endpoints.Titles
{
    public class GetRecentlyUpdatedTitles : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet("titles/latest", HandleGetLatestTitles)
                .WithTags("Titles")
                .WithOpenApi(operation => new(operation)
                {
                    Summary = "Get latest titles",
                    Description = "Get recently updated titles."
                })
                .MapToApiVersion(1)
                .AllowAnonymous();
        }

        private static async Task<IResult> HandleGetLatestTitles(
            GetRecentlyUpdatedTitlesRequest request,
            HttpContext context,
            ISender sender,
            CancellationToken cancellationToken)
        {
            var query = new GetRecentlyUpdatedTitlesQuery(request.Page, request.PageSize);

            var result = await sender.Send(query, cancellationToken);

            return result.IsSuccess ? Results.Ok(result) : ResultExtensions.HandleFailure(result, context);
        }
    }
}
