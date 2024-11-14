using MangaBaseAPI.Domain.Common.Models;

namespace MangaBaseAPI.Domain.Entities
{
    public class Chapter : Entity<Guid>
    {
        public string Name { get; set; } = default!;

        public float Index { get; set; } = 0;

        public int Volume { get; set; } = 0;

        public bool IsDeleted { get; set; } = false;

        public Guid TitleId { get; set; }
        public Title Title { get; set; } = default!;

        public Guid? UploaderId { get; set; }
        public User Uploader { get; set; } = default!;

        public ICollection<ChapterImage> ChapterImages { get; set; } = default!;
    }
}
