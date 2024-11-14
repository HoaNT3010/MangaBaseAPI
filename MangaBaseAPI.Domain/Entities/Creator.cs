using MangaBaseAPI.Domain.Common.Models;

namespace MangaBaseAPI.Domain.Entities
{
    public class Creator : Entity<Guid>
    {
        public string Name { get; set; } = default!;

        public string? Biography { get; set; }

        public ICollection<TitleAuthor> TitleAuthors { get; set; } = default!;
        public ICollection<TitleArtist> TitleArtists { get; set; } = default!;
    }
}
