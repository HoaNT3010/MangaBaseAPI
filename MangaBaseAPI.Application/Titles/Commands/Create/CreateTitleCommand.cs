using MangaBaseAPI.Domain.Abstractions;
using MediatR;

namespace MangaBaseAPI.Application.Titles.Commands.Create
{
    public record CreateTitleCommand(
        string Name,
        string? Description,
        // TitleType enum
        int TitleType,
        // TitleStatus enum
        int TitleStatus,
        DateTimeOffset? PublishedDate,
        List<int>? Genres,
        List<TitleAlternativeName>? AlternativeNames,
        List<Guid>? Authors,
        List<Guid>? Artists,
        Guid UploaderId) : IRequest<Result>;

    public record TitleAlternativeName(
        string Name,
        string LanguageCodeId);
}
