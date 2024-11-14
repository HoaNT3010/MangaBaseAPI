using MangaBaseAPI.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MangaBaseAPI.Persistence.MappingConfigurations
{
    public class TitleRatingConfiguration : IEntityTypeConfiguration<TitleRating>
    {
        public void Configure(EntityTypeBuilder<TitleRating> builder)
        {
            builder.ToTable("TitleRatings");

            builder.HasKey(x => new { x.TitleId, x.UserId });

            builder.HasOne(x => x.Title)
                .WithMany(t => t.TitleRatings)
                .HasForeignKey(x => x.TitleId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(x => x.User)
                .WithMany(u => u.TitleRatings)
                .HasForeignKey(x => x.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
