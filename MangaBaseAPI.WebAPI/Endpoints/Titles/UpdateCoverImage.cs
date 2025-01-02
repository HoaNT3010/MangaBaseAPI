using Carter;
using MangaBaseAPI.Application.Titles.Commands.UpdateCoverImage;
using MangaBaseAPI.Domain.Constants.Authorization;
using MangaBaseAPI.WebAPI.Common;
using MediatR;

namespace MangaBaseAPI.WebAPI.Endpoints.Titles
{
    public class UpdateCoverImage : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPut("titles/{id:guid}/cover", HandleUpdateCoverImage)
                .WithTags("Titles")
                .WithOpenApi(operation => new(operation)
                {
                    Summary = "Update a title's cover image",
                    Description = "Update a title's cover image with provided image file. Only support png, jpeg and webp files."
                })
                .MapToApiVersion(1)
                .RequireAuthorization(Policies.AdminRole)
                .DisableAntiforgery();
        }

        private static async Task<IResult> HandleUpdateCoverImage(
            Guid id,
            IFormFile coverImage,
            ISender sender,
            CancellationToken cancellationToken)
        {
            var command = new UpdateTitleCoverImageCommand(id, coverImage);

            var result = await sender.Send(command, cancellationToken);

            return result.IsSuccess ? Results.Ok(result) : ResultExtensions.HandleFailure(result);
        }
    }
}
