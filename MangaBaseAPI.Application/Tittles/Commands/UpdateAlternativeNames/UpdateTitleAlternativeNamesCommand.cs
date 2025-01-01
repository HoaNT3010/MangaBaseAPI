using MangaBaseAPI.Application.Tittles.Commands.Create;
using MangaBaseAPI.Domain.Abstractions;
using MediatR;

namespace MangaBaseAPI.Application.Tittles.Commands.UpdateAlternativeNames
{
    public record UpdateTitleAlternativeNamesCommand(
        Guid Id,
        List<TitleAlternativeName> AlternativeNames) : IRequest<Result>;
}
