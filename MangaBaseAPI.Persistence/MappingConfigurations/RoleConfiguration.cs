using MangaBaseAPI.Domain.Constants.Role;
using MangaBaseAPI.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MangaBaseAPI.Persistence.MappingConfigurations
{
    public class RoleConfiguration : IEntityTypeConfiguration<Role>
    {
        const string AdminId = "123e4567-e89b-12d3-a456-426614174000";
        const string MemberId = "123e4567-e89b-12d3-a456-426614174001";

        public void Configure(EntityTypeBuilder<Role> builder)
        {
            builder.ToTable("Roles");

            builder.Property(x => x.Id)
                .HasDefaultValueSql("newsequentialid()");

            // Seed data
            builder.HasData(new List<Role>
            {
               new Role()
               {
                   Id = Guid.Parse(AdminId),
                   Name = ApplicationRoles.Administrator,
                   NormalizedName = ApplicationRoles.Administrator.ToLower(),
               },
               new Role()
               {
                   Id = Guid.Parse(MemberId),
                   Name = ApplicationRoles.Member,
                   NormalizedName = ApplicationRoles.Member.ToLower(),
               }
            });
        }
    }
}
