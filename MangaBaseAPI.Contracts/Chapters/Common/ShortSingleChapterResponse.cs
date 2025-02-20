namespace MangaBaseAPI.Contracts.Chapters.Common
{
    public class ShortSingleChapterResponse()
    {
        public Guid Id { get; init; }
        public string Name { get; init; } = null!;
        public float Index { get; init; }
        public int Volume { get; init; } 
        public DateTimeOffset? CreatedDateTime { get; init; }
        public DateTimeOffset? ModifiedDateTime { get; init; }
    }
}
