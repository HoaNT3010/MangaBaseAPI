using MangaBaseAPI.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MangaBaseAPI.Persistence.MappingConfigurations
{
    public class ChapterConfiguration : IEntityTypeConfiguration<Chapter>
    {
        public void Configure(EntityTypeBuilder<Chapter> builder)
        {
            builder.ToTable("Chapters");

            builder.Property(x => x.Name)
                .HasMaxLength(250);

            builder.Property(x => x.CreatedDateTime)
                .HasDefaultValueSql("SYSDATETIMEOFFSET()");

            builder.HasOne(x => x.Title)
                .WithMany(t => t.Chapters)
                .HasForeignKey(x => x.TitleId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(x => x.Uploader)
                .WithMany(u => u.UploadedChapters)
                .HasForeignKey(x => x.UploaderId)
                .OnDelete(DeleteBehavior.SetNull);

            builder.Property(x => x.RowVersion)
                .IsRowVersion();

            builder.Property(x => x.ModifiedDateTime)
                .HasDefaultValueSql("SYSDATETIMEOFFSET()");
        }
    }
}
