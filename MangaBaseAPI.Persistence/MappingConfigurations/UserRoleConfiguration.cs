using MangaBaseAPI.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MangaBaseAPI.Persistence.MappingConfigurations
{
    public class UserRoleConfiguration : IEntityTypeConfiguration<UserRole>
    {
        public void Configure(EntityTypeBuilder<UserRole> builder)
        {
            builder.ToTable("UserRoles");

            // Seed data
            builder.HasData(new List<UserRole>
            {
               new UserRole()
               {
                   UserId = Guid.Parse("11111111-1111-1111-1111-111111111111"),
                   RoleId = Guid.Parse("123e4567-e89b-12d3-a456-426614174000"),
               }
            });
        }
    }
}
