using MangaBaseAPI.Domain.Common.Models;

namespace MangaBaseAPI.Domain.Entities
{
    public class Creator : Entity<Guid>
    {
        public string Name { get; set; } = default!;

        public string? Biography { get; set; }

        public ICollection<TitleAuthor> TitleAuthors { get; set; } = default!;
        public ICollection<TitleArtist> TitleArtists { get; set; } = default!;

        public Creator()
        {
        }

        public Creator(string name,
            string? biography)
        {
            Name = name;
            Biography = biography;
            Id = Guid.NewGuid();
        }

        public Creator(Guid id,
            string name,
            string? biography)
        {
            Id = id;
            Name = name;
            Biography = biography;
        }
    }
}
