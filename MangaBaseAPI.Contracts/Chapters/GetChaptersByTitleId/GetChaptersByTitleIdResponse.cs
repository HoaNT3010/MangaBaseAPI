namespace MangaBaseAPI.Contracts.Chapters.GetChaptersByTitleId
{
    public class GetChaptersByTitleIdResponse
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = default!;
        public float Index { get; set; }
        public int Volume { get; set; }
        public DateTimeOffset? CreatedDateTime { get; set; }
        public DateTimeOffset? ModifiedDateTime { get; set; }
    }
}
