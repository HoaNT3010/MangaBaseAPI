using Carter;

namespace MangaBaseAPI.WebAPI
{
    public static class WebApplicationExtensions
    {
        public static WebApplication RegisterPipelines(this WebApplication application)
        {
            application.UseHttpsRedirection();

            application.UseAuthorization();

            application.MapControllers();

            application.MapCarter();

            return application;
        }

        public static WebApplication AddSwagger(this WebApplication application)
        {
            if (application.Environment.IsDevelopment())
            {
                application.UseSwagger();
                application.UseSwaggerUI();
            }
            return application;
        }

        public static WebApplication UseGlobalErrorHandling(this WebApplication application)
        {
            application.UseExceptionHandler();

            return application;
        }
    }
}
