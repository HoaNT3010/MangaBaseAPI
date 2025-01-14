namespace MangaBaseAPI.Contracts.Creators.GetById
{
    public class GetCreatorByIdResponse
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = default!;
        public string? Biography { get; set; }
        public DateTimeOffset? CreatedDateTime { get; set; }
        public DateTimeOffset? ModifiedDateTime { get; set; }

        public List<CreatorTitle> Publications { get; set; } = default!;
        public List<CreatorTitle> Artworks { get; set; } = default!;
    }

    public record CreatorTitle(
        Guid Id,
        string Name,
        string? CoverImageUrl);
}
