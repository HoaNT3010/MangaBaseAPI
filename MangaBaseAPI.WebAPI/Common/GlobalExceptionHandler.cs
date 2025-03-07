using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using MangaBaseAPI.Domain.Abstractions;

namespace MangaBaseAPI.WebAPI.Common
{
    public class GlobalExceptionHandler : IExceptionHandler
    {
        private readonly ILogger<GlobalExceptionHandler> _logger;
        private const string UnexpectedErrorCode = "Error.GlobalExpceptionHandler.UnexpectedError";

        public GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger)
        {
            _logger = logger;
        }

        public async ValueTask<bool> TryHandleAsync(
            HttpContext httpContext,
            Exception exception,
            CancellationToken cancellationToken)
        {
            _logger.LogError(exception, "Unexpected exception occurred: {Message}", exception.Message);

            var extension = new Dictionary<string, object?>();
            extension.TryAdd("requestId", httpContext.TraceIdentifier);
            Activity? activity = httpContext.Features.Get<IHttpActivityFeature>()?.Activity;
            extension.TryAdd("traceId", activity?.Id);
            var error = Error.Failure(UnexpectedErrorCode, $"Unexpected error occurred: {exception.Message}");
            extension.TryAdd("errors", new[] { error });

            var problemDetails = new ProblemDetails
            {
                Status = StatusCodes.Status500InternalServerError,
                Title = "Internal Server Error",
                Type = "https://datatracker.ietf.org/doc/html/rfc7231#section-6.6.1",
                Detail = exception.Message,
                Extensions = extension,
                Instance = $"{httpContext.Request.Method} {httpContext.Request.Path}",
            };

            httpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;

            await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);

            return true;
        }
    }
}
