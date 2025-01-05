namespace MangaBaseAPI.Contracts.Chapters.Create
{
    public record CreateChapterRequest(
        string Name,
        float Index,
        int Volume);
}
