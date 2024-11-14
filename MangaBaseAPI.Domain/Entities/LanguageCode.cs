using MangaBaseAPI.Domain.Common.Models;

namespace MangaBaseAPI.Domain.Entities
{
    public class LanguageCode : IHasKey<string>
    {
        public string Id { get; set; } = null!;
        public string EnglishName { get; set; } = null!;

        public ICollection<AlternativeName> AlternativeNames { get; set; } = default!;
    }
}
