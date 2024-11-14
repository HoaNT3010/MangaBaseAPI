using MangaBaseAPI.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MangaBaseAPI.Persistence.MappingConfigurations
{
    public class TitleArtistConfiguration : IEntityTypeConfiguration<TitleArtist>
    {
        public void Configure(EntityTypeBuilder<TitleArtist> builder)
        {
            builder.ToTable("TitleArtists");

            builder.HasKey(x => new { x.TitleId, x.ArtistId });

            builder.HasOne(x => x.Title)
                .WithMany(t => t.TitleArtists)
                .HasForeignKey(x => x.TitleId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(x => x.Artist)
                .WithMany(c => c.TitleArtists)
                .HasForeignKey(x => x.ArtistId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
