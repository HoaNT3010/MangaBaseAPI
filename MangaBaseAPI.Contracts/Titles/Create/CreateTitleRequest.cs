namespace MangaBaseAPI.Contracts.Titles.Create
{
    public record CreateTitleRequest(
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
        List<Guid>? Artists);

    public record TitleAlternativeName(
        string Name,
        string LanguageCodeId);
}
