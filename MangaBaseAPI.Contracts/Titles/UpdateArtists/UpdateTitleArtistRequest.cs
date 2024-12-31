namespace MangaBaseAPI.Contracts.Titles.UpdateArtists
{
    public record UpdateTitleArtistRequest(
        List<Guid> Artists);
}
