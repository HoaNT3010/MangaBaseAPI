using MangaBaseAPI.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MangaBaseAPI.Persistence.MappingConfigurations
{
    public class LanguageCodeConfiguration : IEntityTypeConfiguration<LanguageCode>
    {
        public void Configure(EntityTypeBuilder<LanguageCode> builder)
        {
            builder.ToTable("LanguageCodes");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id)
                .HasMaxLength(2);

            builder.Property(x => x.EnglishName)
                .HasMaxLength(50);
        }
    }
}
