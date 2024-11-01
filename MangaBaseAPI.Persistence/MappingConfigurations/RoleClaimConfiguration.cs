using MangaBaseAPI.Domain.Constants.Role;
using MangaBaseAPI.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MangaBaseAPI.Persistence.MappingConfigurations
{
    public class RoleClaimConfiguration : IEntityTypeConfiguration<RoleClaim>
    {
        const string AdminId = "123e4567-e89b-12d3-a456-426614174000";
        const string MemberId = "123e4567-e89b-12d3-a456-426614174001";

        public void Configure(EntityTypeBuilder<RoleClaim> builder)
        {
            builder.ToTable("RoleClaims");

            // Seed data
            builder.HasData(new List<RoleClaim>
            {
               // Admin
               new RoleClaim()
               {
                   Id = 1,
                   RoleId = Guid.Parse(AdminId),
                   ClaimType = RoleClaimType.AccessLevel,
                   ClaimValue = RoleClaimValue.AccessLevelAdmin,
               },
               new RoleClaim()
               {
                   Id = 2,
                   RoleId = Guid.Parse(AdminId),
                   ClaimType = RoleClaimType.CanManageUsers,
                   ClaimValue = RoleClaimValue.CanManageUsers,
               },
               new RoleClaim()
               {
                   Id = 3,
                   RoleId = Guid.Parse(AdminId),
                   ClaimType = RoleClaimType.CanManageContent,
                   ClaimValue = RoleClaimValue.CanManageUsers
               },
               // Member
               new RoleClaim()
               {
                   Id = 4,
                   RoleId = Guid.Parse(MemberId),
                   ClaimType = RoleClaimType.AccessLevel,
                   ClaimValue = RoleClaimValue.AccessLevelMember,
               },
               new RoleClaim()
               {
                   Id = 5,
                   RoleId = Guid.Parse(MemberId),
                   ClaimType = RoleClaimType.CanManageUsers,
                   ClaimValue = RoleClaimValue.CannotManageUsers,
               },
               new RoleClaim()
               {
                   Id = 6,
                   RoleId = Guid.Parse(MemberId),
                   ClaimType = RoleClaimType.CanManageContent,
                   ClaimValue = RoleClaimValue.CannotManageContent,
               }
            });
        }
    }
}
