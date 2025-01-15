using MangaBaseAPI.Domain.Entities;
using MangaBaseAPI.Domain.Repositories;
using MangaBaseAPI.Persistence.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace MangaBaseAPI.Persistence
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddPersistence(this IServiceCollection services, string connectionString)
        {
            services.AddDbContext<MangaBaseDbContext>(options =>
            {
                options.UseSqlServer(connectionString);
            });

            services.AddIdentity<User, Role>(options =>
            {
                options.SignIn.RequireConfirmedAccount = false;
                options.SignIn.RequireConfirmedEmail = false;
                options.SignIn.RequireConfirmedPhoneNumber = false;

                options.User.RequireUniqueEmail = true;

                options.Lockout.AllowedForNewUsers = true;
                options.Lockout.MaxFailedAccessAttempts = 5;
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(3);
            })
            .AddSignInManager<SignInManager<User>>()
            .AddEntityFrameworkStores<MangaBaseDbContext>()
            .AddDefaultTokenProviders();

            services.AddUnitOfWork();
            services.AddDbContextHealthCheck();

            return services;
        }

        private static IServiceCollection AddUnitOfWork(this IServiceCollection services)
        {
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            // Add repositories
            services.AddRepositories();

            return services;
        }

        private static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            services.AddScoped<IUserTokenRepository, UserTokenRepository>();
            services.AddScoped<IGenreRepository, GenreRepository>();
            services.AddScoped<ILanguageCodeRepository, LanguageCodeRepository>();
            services.AddScoped<IAlternativeNameRepository, AlternativeNameRepository>();
            services.AddScoped<IChapterImageRepository, ChapterImageRepository>();
            services.AddScoped<IChapterRepository, ChapterRepository>();
            services.AddScoped<ICreatorRepository, CreatorRepository>();
            services.AddScoped<ITitleArtistRepository, TitleArtistRepository>();
            services.AddScoped<ITitleAuthorRepository, TitleAuthorRepository>();
            services.AddScoped<ITitleGenreRepository, TitleGenreRepository>();
            services.AddScoped<ITitleRatingRepository, TitleRatingRepository>();
            services.AddScoped<ITitleRepository, TitleRepository>();

            return services;
        }

        private static IServiceCollection AddDbContextHealthCheck(this IServiceCollection services)
        {
            services.AddHealthChecks()
                .AddDbContextCheck<MangaBaseDbContext>();

            return services;
        }
    }
}
