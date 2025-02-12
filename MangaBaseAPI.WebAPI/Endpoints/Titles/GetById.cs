using Carter;
using MangaBaseAPI.Application.Titles.Commands.Create;
using MangaBaseAPI.Application.Titles.Queries.GetById;
using MangaBaseAPI.Contracts.Titles.Create;
using MangaBaseAPI.WebAPI.Common;
using MediatR;

namespace MangaBaseAPI.WebAPI.Endpoints.Titles
{
    public class GetById : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet("titles/{id:guid}", HandleGetById)
                .WithTags("Titles")
                .WithOpenApi(operation => new(operation)
                {
                    Summary = "Get title by title's ID",
                    Description = "Retrieve information of a title with a given ID"
                })
                .MapToApiVersion(1)
                .AllowAnonymous();
        }

        private static async Task<IResult> HandleGetById(
            Guid id,
            HttpContext context,
            ISender sender,
            CancellationToken cancellationToken)
        {
            var query = new GetTitleByIdQuery(id);

            var result = await sender.Send(query, cancellationToken);

            return result.IsSuccess ? Results.Ok(result) : ResultExtensions.HandleFailure(result, context);
        }
    }
}
