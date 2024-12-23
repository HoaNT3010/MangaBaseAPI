using MangaBaseAPI.Domain.Common.Models;

namespace MangaBaseAPI.Domain.Entities
{
    public class AlternativeName : IHasKey<int>
    {
        public int Id { get; set; }

        public string Name { get; set; } = default!;

        public string? LanguageCodeId { get; set; }
        public LanguageCode LanguageCode { get; set; } = default!;

        public Guid TitleId { get; set; }
        public Title Title { get; set; } = default!;

        public AlternativeName()
        {
        }

        public AlternativeName(Guid titleId,
            string name,
            string languageCodeId)
        {
            Name = name;
            LanguageCodeId = languageCodeId;
            TitleId = titleId;
        }
    }
}
