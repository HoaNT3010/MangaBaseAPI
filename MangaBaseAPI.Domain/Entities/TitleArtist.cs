namespace MangaBaseAPI.Domain.Entities
{
    public class TitleArtist
    {
        public Guid TitleId { get; set; }
        public Title Title { get; set; } = default!;

        public Guid ArtistId { get; set; }
        public Creator Artist { get; set; } = default!;
    }
}
