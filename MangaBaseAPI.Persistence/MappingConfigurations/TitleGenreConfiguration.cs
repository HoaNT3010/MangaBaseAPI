using MangaBaseAPI.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MangaBaseAPI.Persistence.MappingConfigurations
{
    public class TitleGenreConfiguration : IEntityTypeConfiguration<TitleGenre>
    {
        public void Configure(EntityTypeBuilder<TitleGenre> builder)
        {
            builder.ToTable("TitleGenres");

            builder.HasKey(x => new { x.TitleId, x.GenreId });

            builder.HasOne(x => x.Title)
                .WithMany(t => t.TitleGenres)
                .HasForeignKey(x => x.TitleId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(x => x.Genre)
                .WithMany(g => g.TitleGenres)
                .HasForeignKey(x => x.GenreId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
