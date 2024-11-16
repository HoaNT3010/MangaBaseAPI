using Carter;
using MangaBaseAPI.Application.Genres.Queries.GetAll;
using MangaBaseAPI.WebAPI.Common;
using MediatR;

namespace MangaBaseAPI.WebAPI.Endpoints.Genres
{
    public class GetAll : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet("genres", HandleGetAllGenres)
                .WithTags("Genres")
                .WithOpenApi(operation => new(operation)
                {
                    Summary = "Get all genres",
                    Description = "Return a collection contains all manga's genres listed in the system"
                })
                .MapToApiVersion(1);
        }

        private static async Task<IResult> HandleGetAllGenres(
            ISender sender,
            CancellationToken cancellationToken)
        {
            var query = new GetAllGenresQuery();

            var result = await sender.Send(query, cancellationToken);

            return result.IsSuccess ? Results.Ok(result) : ResultExtensions.HandleFailure(result);
        }
    }
}
