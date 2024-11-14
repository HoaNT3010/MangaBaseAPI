using MangaBaseAPI.Domain.Common.Models;

namespace MangaBaseAPI.Domain.Entities
{
    public class Genre : IHasKey<int>
    {
        public int Id { get; set; }

        public string Name { get; set; } = null!;

        public ICollection<TitleGenre> TitleGenres { get; set; } = default!;
    }
}
