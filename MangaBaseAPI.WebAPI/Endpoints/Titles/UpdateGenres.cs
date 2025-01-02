using Carter;
using MangaBaseAPI.Application.Titles.Commands.UpdateGenres;
using MangaBaseAPI.Contracts.Titles.UpdateGenres;
using MangaBaseAPI.Domain.Constants.Authorization;
using MangaBaseAPI.WebAPI.Common;
using MediatR;

namespace MangaBaseAPI.WebAPI.Endpoints.Titles
{
    public class UpdateGenres : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPut("titles/{id:guid}/genres", HandleUpdateGenres)
                .WithTags("Titles")
                .WithOpenApi(operation => new(operation)
                {
                    Summary = "Update title's genres",
                    Description = "Update title's list of genres"
                })
                .MapToApiVersion(1)
                .RequireAuthorization(Policies.AdminRole);
        }

        private static async Task<IResult> HandleUpdateGenres(
            Guid id,
            UpdateTitleGenresRequest request,
            ISender sender,
            CancellationToken cancellationToken)
        {
            var command = new UpdateTitleGenresCommand(id, request.Genres);

            var result = await sender.Send(command, cancellationToken);

            return result.IsSuccess ? Results.NoContent() : ResultExtensions.HandleFailure(result);
        }
    }
}
