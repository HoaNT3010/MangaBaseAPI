using MangaBaseAPI.Domain.Common.Models;

namespace MangaBaseAPI.Domain.Entities
{
    public class ChapterImage : IHasKey<int>
    {
        public int Id { get; set; }
        public string Url { get; set; } = default!;

        public Guid ChapterId { get; set; }
        public Chapter Chapter { get; set; } = default!;

        public ChapterImage()
        {
        }

        public ChapterImage(Guid chapterId,
            string url)
        {
            Url = url;
            ChapterId = chapterId;
        }
    }
}
