using MangaBaseAPI.Contracts.Chapters.Common;

namespace MangaBaseAPI.Contracts.Titles.Common
{
    public class ShortTitleResponse
    {
        public Guid Id { get; init; }
        public string Name { get; init; } = null!;
        public string Type { get; init; } = null!;
        public string Status { get; init; } = null!;
        public DateTimeOffset CreatedDateTime { get; init; }
        public DateTimeOffset? ModifiedDateTime { get; init; }
        public string? CoverImageUrl { get; init; }
        public ShortSingleChapterResponse? LatestChapter { get; init; }
    }
}
