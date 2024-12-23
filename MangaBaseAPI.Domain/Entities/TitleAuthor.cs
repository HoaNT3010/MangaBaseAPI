namespace MangaBaseAPI.Domain.Entities
{
    public class TitleAuthor
    {
        public Guid TitleId { get; set; }
        public Title Title { get; set; } = default!;

        public Guid AuthorId { get; set; }
        public Creator Author { get; set; } = default!;

        public TitleAuthor()
        {
        }

        public TitleAuthor(
            Guid titleId,
            Guid creatorId)
        {
            TitleId = titleId;
            AuthorId = creatorId;
        }
    }
}
