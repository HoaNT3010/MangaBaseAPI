using MangaBaseAPI.Domain.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace MangaBaseAPI.Persistence
{
    public class MangaBaseDbContext : IdentityDbContext<
        User,
        Role,
        Guid,
        UserClaim,
        UserRole,
        UserLogin,
        RoleClaim,
        UserToken>
    {
        public MangaBaseDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<PasswordHistory> PasswordHistories { get; set; }
        public DbSet<LanguageCode> LanguageCodes { get; set; }
        public DbSet<Genre> Genres { get; set; }
        public DbSet<Title> Titles { get; set; }
        public DbSet<TitleGenre> TitleGenres { get; set; }
        public DbSet<TitleRating> TitleRatings { get; set; }
        public DbSet<AlternativeName> AlternativeNames { get; set; }
        public DbSet<Chapter> Chapters { get; set; }
        public DbSet<ChapterImage> ChapterImages { get; set; }
        public DbSet<Creator> Creators { get; set; }
        public DbSet<TitleAuthor> TitleAuthors { get; set; }
        public DbSet<TitleArtist> TitleArtists { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
    }
}
