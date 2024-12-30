using Carter;
using MangaBaseAPI.Application.Tittles.Commands.UpdateAuthors;
using MangaBaseAPI.Contracts.Titles.UpdateAuthors;
using MangaBaseAPI.Domain.Constants.Authorization;
using MangaBaseAPI.WebAPI.Common;
using MediatR;

namespace MangaBaseAPI.WebAPI.Endpoints.Titles
{
    public class UpdateAuthors : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPut("titles/{id:guid}/authors", HandleUpdateAuthors)
                .WithTags("Titles")
                .WithOpenApi(operation => new(operation)
                {
                    Summary = "Update title's authors",
                    Description = "Update title's list of authors"
                })
                .MapToApiVersion(1)
                .RequireAuthorization(Policies.AdminRole);
        }

        private static async Task<IResult> HandleUpdateAuthors(
            Guid id,
            UpdateTitleAuthorsRequest request,
            ISender sender,
            CancellationToken cancellationToken)
        {
            var command = new UpdateTitleAuthorsCommand(id, request.Authors);

            var result = await sender.Send(command, cancellationToken);

            return result.IsSuccess ? Results.NoContent() : ResultExtensions.HandleFailure(result);
        }
    }
}
