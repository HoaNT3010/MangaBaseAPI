using MangaBaseAPI.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MangaBaseAPI.Persistence.MappingConfigurations
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("Users");

            builder.Property(x => x.Id)
                .HasDefaultValueSql("newsequentialid()");

            builder.Property(x => x.FirstName)
                .HasMaxLength(50);

            builder.Property(x => x.LastName)
                .HasMaxLength(50);

            builder.Property(x => x.CreatedDateTime)
                .HasDefaultValueSql("SYSDATETIMEOFFSET()");

            // Seed data
            builder.HasData(new List<User>
            {
               new User
               {
                   Id = Guid.Parse("11111111-1111-1111-1111-111111111111"),
                   UserName = "GibleBC2",
                   NormalizedUserName = "giblebc2",
                   Email = "hoa41300@gmail.com",
                   NormalizedEmail = "hoa41300@gmail.com",
                   LockoutEnabled = false,
                   PasswordHash = "AQAAAAEAACcQAAAAELBcKuXWkiRQEYAkD/qKs9neac5hxWs3bkegIHpGLtf+zFHuKnuI3lBqkWO9TMmFAQ==",   // v*7Un8b4rcN@<-RN
                   SecurityStamp = "5M2QLL65J6H6VFIS7VZETKXY27KNVVYJ",
                   FirstName = "Hòa",
                   LastName = "Nguyễn Thái"
               }
            });
        }
    }
}
