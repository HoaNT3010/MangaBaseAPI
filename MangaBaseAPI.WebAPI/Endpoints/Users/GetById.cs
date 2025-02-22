using Carter;
using MangaBaseAPI.Application.Users.Queries.GetById;
using MangaBaseAPI.WebAPI.Common;
using MediatR;

namespace MangaBaseAPI.WebAPI.Endpoints.Users
{
    public class GetById : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet("users/{id:guid}", HandleGetById)
                .WithTags("Users")
                .WithOpenApi(operation => new(operation)
                {
                    Summary = "Get user by user's ID",
                    Description = "Retrieve information of an user with a given ID"
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
            var query = new GetUserByIdQuery(id);

            var result = await sender.Send(query, cancellationToken);

            return result.IsSuccess ? Results.Ok(result) : ResultExtensions.HandleFailure(result, context);
        }
    }
}
