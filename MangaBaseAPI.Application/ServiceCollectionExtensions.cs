using FluentValidation;
using MangaBaseAPI.Application.Common.Behaviors;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace MangaBaseAPI.Application
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddMediatR(options =>
            {
                options.RegisterServicesFromAssembly(typeof(ServiceCollectionExtensions).Assembly);
            });

            services.AddFluentValidationPipeline();
            services.AddLoggingPipelineBehavior();
            services.ConfigureAutoMapper();

            return services;
        }

        private static IServiceCollection AddFluentValidationPipeline(this IServiceCollection services)
        {
            services.AddScoped(
                typeof(IPipelineBehavior<,>),
                typeof(ValidationBehavior<,>));

            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly(), includeInternalTypes: true);

            return services;
        }

        private static IServiceCollection AddLoggingPipelineBehavior(this IServiceCollection services)
        {
            services.AddScoped(
                typeof(IPipelineBehavior<,>),
                typeof(LoggingPipelineBehavior<,>));

            return services;
        }

        private static IServiceCollection ConfigureAutoMapper(this IServiceCollection services) 
        {
            services.AddAutoMapper(typeof(ServiceCollectionExtensions).Assembly);

            return services;
        }
    }
}
