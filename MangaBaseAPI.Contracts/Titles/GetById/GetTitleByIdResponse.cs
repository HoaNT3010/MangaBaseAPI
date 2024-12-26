namespace MangaBaseAPI.Contracts.Titles.GetById
{
    public class TitleCreator
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = default!;
    }

    public class TitleAlternativeName
    {
        public int Id { get; set; }
        public string Name { get; set; } = default!;
        public string? LanguageCodeId { get; set; }
        public string LanguageEnglishName { get; set; } = default!;
    }

    public class GetTitleByIdResponse
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = default!;
        public string? Description { get; set; }
        public string Type { get; set; } = default!;
        public string Status { get; set; } = default!;
        public DateTimeOffset? PublishedDate { get; set; }
        public float AverageRating { get; set; } = default!;
        public string? CoverImageUrl { get; set; }
        public Guid? UploaderId { get; set; }
        public List<string>? Genres { get; set; }
        public List<TitleAlternativeName> AlternativeNames { get; set; } = default!;
        public List<TitleCreator> Authors { get; set; } = default!;
        public List<TitleCreator> Artists { get; set; } = default!;
    }
}
