using Carter;
using MangaBaseAPI.Application.Creators.Queries.GetById;
using MangaBaseAPI.WebAPI.Common;
using MediatR;

namespace MangaBaseAPI.WebAPI.Endpoints.Creators
{
    public class GetById : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet("creators/{id:guid}", HandleGetCreatorById)
                .WithTags("Creators")
                .WithOpenApi(operation => new(operation)
                {
                    Summary = "Get creator by ID",
                    Description = "Retrieve data of a creator with creator's ID. The data consists of basic information and creator's works."
                })
                .MapToApiVersion(1)
                .AllowAnonymous();
        }

        private static async Task<IResult> HandleGetCreatorById(
            Guid id,
            HttpContext context,
            ISender sender,
            CancellationToken cancellationToken)
        {
            var query = new GetCreatorByIdQuery(id);

            var result = await sender.Send(query, cancellationToken);

            return result.IsSuccess ? Results.Ok(result) : ResultExtensions.HandleFailure(result, context);
        }
    }
}
