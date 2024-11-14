using MangaBaseAPI.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MangaBaseAPI.Persistence.MappingConfigurations
{
    public class TitleAuthorConfiguration : IEntityTypeConfiguration<TitleAuthor>
    {
        public void Configure(EntityTypeBuilder<TitleAuthor> builder)
        {
            builder.ToTable("TitleAuthors");

            builder.HasKey(x => new { x.TitleId, x.AuthorId });

            builder.HasOne(x => x.Title)
                .WithMany(t => t.TitleAuthors)
                .HasForeignKey(x => x.TitleId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(x => x.Author)
                .WithMany(c => c.TitleAuthors)
                .HasForeignKey(x => x.AuthorId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
