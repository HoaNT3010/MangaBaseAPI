using Carter;
using MangaBaseAPI.Application.Titles.Commands.UpdateArtists;
using MangaBaseAPI.Contracts.Titles.UpdateArtists;
using MangaBaseAPI.Domain.Constants.Authorization;
using MangaBaseAPI.WebAPI.Common;
using MediatR;

namespace MangaBaseAPI.WebAPI.Endpoints.Titles
{
    public class UpdateArtists : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPut("titles/{id:guid}/artists", HandleUpdateArtists)
                .WithTags("Titles")
                .WithOpenApi(operation => new(operation)
                {
                    Summary = "Update title's artists",
                    Description = "Update title's list of artists"
                })
                .MapToApiVersion(1)
                .RequireAuthorization(Policies.AdminRole);
        }

        private static async Task<IResult> HandleUpdateArtists(
            Guid id,
            UpdateTitleArtistRequest request,
            HttpContext context,
            ISender sender,
            CancellationToken cancellationToken)
        {
            var command = new UpdateTitleArtistsCommand(id, request.Artists);

            var result = await sender.Send(command, cancellationToken);

            return result.IsSuccess ? Results.NoContent() : ResultExtensions.HandleFailure(result, context);
        }
    }
}
