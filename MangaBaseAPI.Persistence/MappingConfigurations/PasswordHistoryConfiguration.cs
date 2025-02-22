using MangaBaseAPI.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MangaBaseAPI.Persistence.MappingConfigurations
{
    public class PasswordHistoryConfiguration : IEntityTypeConfiguration<PasswordHistory>
    {
        public void Configure(EntityTypeBuilder<PasswordHistory> builder)
        {
            builder.ToTable("PasswordHistories");

            builder.Property(x => x.Id)
                .HasDefaultValueSql("newsequentialid()");

            builder.Property(x => x.CreatedDateTime)
                .HasDefaultValueSql("SYSDATETIMEOFFSET()");

            builder.HasOne(x => x.User)
                .WithMany(u => u.PasswordHistories)
                .HasForeignKey(x => x.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Property(x => x.RowVersion)
                .IsRowVersion();

            builder.Property(x => x.ModifiedDateTime)
                .HasDefaultValueSql("SYSDATETIMEOFFSET()");

            // Seed data
            builder.HasData(new List<PasswordHistory>()
            {
                new PasswordHistory{
                    Id = Guid.Parse("22222222-1111-1111-1111-111111111111"),
                    UserId = Guid.Parse("11111111-1111-1111-1111-111111111111"),
                    PasswordHash = "AQAAAAIAAYagAAAAENX7BIlY1gy8Getg2rmVWj0zLEDmvNY8m7TEJETG6JYBfWbiKN41/MgUaiU8N03GRw==",
                }
            });
        }
    }
}
