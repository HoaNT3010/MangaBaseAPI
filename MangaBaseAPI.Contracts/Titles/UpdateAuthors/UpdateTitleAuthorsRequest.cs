namespace MangaBaseAPI.Contracts.Titles.UpdateAuthors
{
    public record UpdateTitleAuthorsRequest(
        List<Guid> Authors);
}
