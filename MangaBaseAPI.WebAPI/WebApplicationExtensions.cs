using Asp.Versioning.Builder;
using Asp.Versioning;
using Carter;
using Serilog;
using MangaBaseAPI.WebAPI.Middlewares;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using HealthChecks.UI.Client;

namespace MangaBaseAPI.WebAPI
{
    public static class WebApplicationExtensions
    {
        public static WebApplication RegisterPipelines(this WebApplication application)
        {
            application.UseHttpsRedirection();

            application.UseAuthorization();

            application.MapMinimalApiEndpoints();

            application.UseSerilogRequestLogging();

            return application;
        }

        public static WebApplication AddSwagger(this WebApplication application)
        {
            application.UseSwagger();
            application.UseSwaggerUI(options =>
            {
                var descriptions = application.DescribeApiVersions();
                foreach (var description in descriptions)
                {
                    string url = $"/swagger/{description.GroupName}/swagger.json";
                    string name = description.GroupName.ToUpperInvariant();

                    options.SwaggerEndpoint(url, name);
                }
            });
            return application;
        }

        public static WebApplication UseGlobalErrorHandling(this WebApplication application)
        {
            application.UseExceptionHandler();

            return application;
        }

        private static WebApplication MapMinimalApiEndpoints(this WebApplication application)
        {
            // Map minimal api endpoints with api versioning
            ApiVersionSet apiVersionSet = application.NewApiVersionSet()
                .HasApiVersion(new ApiVersion(1))
                .HasApiVersion(new ApiVersion(2))
                .ReportApiVersions()
                .Build();

            RouteGroupBuilder versionedGroup = application
                .MapGroup("api/v{apiVersion:apiVersion}")
                .WithApiVersionSet(apiVersionSet);

            versionedGroup.MapCarter();

            return application;
        }

        public static WebApplication RegisterMiddlewares(this WebApplication application)
        {
            application.UseMiddleware<UserClaimsMiddleware>();

            return application;
        }

        public static WebApplication UseHealthChecks(this WebApplication application)
        {
            application.MapHealthChecks("health", new HealthCheckOptions
            {
                ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
            });

            return application;
        }
    }
}
