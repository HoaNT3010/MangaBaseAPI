using MangaBaseAPI.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MangaBaseAPI.Persistence.MappingConfigurations
{
    public class ChapterImageConfiguration : IEntityTypeConfiguration<ChapterImage>
    {
        public void Configure(EntityTypeBuilder<ChapterImage> builder)
        {
            builder.ToTable("ChapterImages");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id)
                .ValueGeneratedOnAdd();

            builder.HasOne(x => x.Chapter)
                .WithMany(c => c.ChapterImages)
                .HasForeignKey(x => x.ChapterId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
