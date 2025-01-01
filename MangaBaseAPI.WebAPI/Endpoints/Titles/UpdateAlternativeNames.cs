using Carter;
using MangaBaseAPI.Application.Tittles.Commands.Create;
using MangaBaseAPI.Application.Tittles.Commands.UpdateAlternativeNames;
using MangaBaseAPI.Contracts.Titles.UpdateAlternativeNames;
using MangaBaseAPI.Domain.Constants.Authorization;
using MangaBaseAPI.WebAPI.Common;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace MangaBaseAPI.WebAPI.Endpoints.Titles
{
    public class UpdateAlternativeNames : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPut("titles/{id:guid}/names", HandleUpdateAlternativeNames)
                .WithTags("Titles")
                .WithOpenApi(operation => new(operation)
                {
                    Summary = "Update title's alternative names",
                    Description = "Update title's list of alternative names"
                })
                .MapToApiVersion(1)
                .RequireAuthorization(Policies.AdminRole);
        }

        private static async Task<IResult> HandleUpdateAlternativeNames(
            [FromRoute] Guid id,
            [FromBody] UpdateTitleAlternativeNamesRequest request,
            ISender sender,
            CancellationToken cancellationToken)
        {
            var command = new UpdateTitleAlternativeNamesCommand(id,
                request.AlternativeNames == null
                ? new List<TitleAlternativeName>()
                : request.AlternativeNames.Select(source => new TitleAlternativeName(source.Name, source.LanguageCodeId)).ToList());

            var result = await sender.Send(command, cancellationToken);

            return result.IsSuccess ? Results.NoContent() : ResultExtensions.HandleFailure(result);
        }
    }
}
