using Carter;
using MangaBaseAPI.Application.Creators.Queries;
using MangaBaseAPI.Contracts.Creators.Search;
using MangaBaseAPI.WebAPI.Common;
using MediatR;

namespace MangaBaseAPI.WebAPI.Endpoints.Creators
{
    public class SearchByName : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet("creators/search", HandleSearchByName)
                .WithTags("Creators")
                .WithOpenApi(operation => new(operation)
                {
                    Summary = "Search creators by name",
                    Description = "Search creators by name with given keyword. Return a paged list of creators."
                })
                .MapToApiVersion(1)
                .AllowAnonymous();
        }

        private static async Task<IResult> HandleSearchByName(
            SearchCreatorByNameRequest request,
            ISender sender,
            CancellationToken cancellationToken)
        {
            var query = new SearchCreatorByNameQuery(request.Keyword, request.Page, request.PageSize);

            var result = await sender.Send(query, cancellationToken);

            return result.IsSuccess ? Results.Ok(result) : ResultExtensions.HandleFailure(result);
        }
    }
}
