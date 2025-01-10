namespace MangaBaseAPI.Contracts.Chapters.GetById
{
    public class GetChapterByIdResponse
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = default!;
        public float Index { get; set; }
        public int Volume { get; set; }
        public int TotalPages { get; set; }
        public DateTimeOffset? CreatedDateTime { get; set; }
        public DateTimeOffset? ModifiedDateTime { get; set; }

        public List<ChapterImageResponse> Images { get; set; } = default!;
        public ChapterTitleResponse Title { get; set; } = default!;
    }

    public class ChapterImageResponse
    {
        public string Url { get; set; } = default!;
        public int Index { get; set; }
    }

    public class ChapterTitleResponse
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = default!;
    }
}
