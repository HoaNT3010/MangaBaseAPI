using Carter;
using MangaBaseAPI.Application.Creators.Commands.Create;
using MangaBaseAPI.Contracts.Creators.Create;
using MangaBaseAPI.Domain.Constants.Authorization;
using MangaBaseAPI.WebAPI.Common;
using MediatR;

namespace MangaBaseAPI.WebAPI.Endpoints.Creators
{
    public class Create : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPost("creators", HandleCreateCreator)
                .WithTags("Creators")
                .WithOpenApi(operation => new(operation)
                {
                    Summary = "Create new creator",
                    Description = "Create a new creator (author/artist) with provided data"
                })
                .MapToApiVersion(1)
                .RequireAuthorization(Policies.AdminRole);
        }

        private static async Task<IResult> HandleCreateCreator(
            CreateCreatorRequest request,
            HttpContext context,
            ISender sender,
            CancellationToken cancellationToken)
        {
            var command = new CreateCreatorCommand(request.Name, request.Biography);

            var result = await sender.Send(command, cancellationToken);

            return result.IsSuccess ? Results.Ok(result) : ResultExtensions.HandleFailure(result, context);
        }
    }
}
