using MangaBaseAPI.Contracts.Titles.Create;

namespace MangaBaseAPI.Contracts.Titles.UpdateAlternativeNames
{
    public record UpdateTitleAlternativeNamesRequest(
        List<TitleAlternativeName>? AlternativeNames);
}
