using MangaBaseAPI.Application.Titles.Commands.Create;
using MangaBaseAPI.Domain.Abstractions;
using MediatR;

namespace MangaBaseAPI.Application.Titles.Commands.UpdateAlternativeNames
{
    public record UpdateTitleAlternativeNamesCommand(
        Guid Id,
        List<TitleAlternativeName> AlternativeNames) : IRequest<Result>;
}
