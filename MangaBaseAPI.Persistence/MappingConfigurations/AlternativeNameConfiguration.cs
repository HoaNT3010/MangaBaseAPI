using MangaBaseAPI.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MangaBaseAPI.Persistence.MappingConfigurations
{
    public class AlternativeNameConfiguration : IEntityTypeConfiguration<AlternativeName>
    {
        public void Configure(EntityTypeBuilder<AlternativeName> builder)
        {
            builder.ToTable("AlternativeNames");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id)
                .ValueGeneratedOnAdd();

            builder.Property(x => x.Name)
                .HasMaxLength(250);

            builder.HasOne(x => x.Title)
                .WithMany(t => t.AlternativeNames)
                .HasForeignKey(x => x.TitleId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(x => x.LanguageCode)
                .WithMany(l => l.AlternativeNames)
                .HasForeignKey(x => x.LanguageCodeId)
                .OnDelete(DeleteBehavior.SetNull);
        }
    }
}
