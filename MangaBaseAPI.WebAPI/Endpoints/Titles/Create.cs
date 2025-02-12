using Carter;
using MangaBaseAPI.Application.Titles.Commands.Create;
using MangaBaseAPI.Contracts.Titles.Create;
using MangaBaseAPI.Domain.Constants.Authorization;
using MangaBaseAPI.WebAPI.Common;
using MediatR;
using TitleAlternativeName = MangaBaseAPI.Application.Titles.Commands.Create.TitleAlternativeName;

namespace MangaBaseAPI.WebAPI.Endpoints.Titles
{
    public class Create : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPost("titles", HandleCreateTitle)
                .WithTags("Titles")
                .WithOpenApi(operation => new(operation)
                {
                    Summary = "Create new title",
                    Description = "Create a new title with provided data"
                })
                .MapToApiVersion(1)
                .RequireAuthorization(Policies.AdminRole);
        }

        private static async Task<IResult> HandleCreateTitle(
            CreateTitleRequest request,
            HttpContext context,
            ISender sender,
            CancellationToken cancellationToken)
        {

            if (!context.Items.TryGetValue("UserId", out var userId))
            {
                return Results.Unauthorized();
            }
            var command = new CreateTitleCommand(
                request.Name,
                request.Description,
                request.TitleType,
                request.TitleStatus,
                request.PublishedDate,
                request.Genres,
                request.AlternativeNames?.Select(source => new TitleAlternativeName(source.Name, source.LanguageCodeId)).ToList(),
                request.Authors, request.Artists,
                Guid.Parse(userId!.ToString()!));

            var result = await sender.Send(command, cancellationToken);

            return result.IsSuccess ? Results.Created(result.Value.Location, result) : ResultExtensions.HandleFailure(result, context);
        }
    }
}
