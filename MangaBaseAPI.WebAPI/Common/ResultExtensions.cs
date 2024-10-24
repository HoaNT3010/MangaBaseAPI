using MangaBaseAPI.Domain.Abstractions;
using Microsoft.AspNetCore.Mvc;

namespace MangaBaseAPI.WebAPI.Common
{
    public static class ResultExtensions
    {
        public static IResult HandleFailure(Result result) =>
            result switch
            {
                { IsSuccess: true } => throw new InvalidOperationException(),
                IValidationResult validationResult => Results.BadRequest(
                    CreateProblemDetails(
                        "Validation Error(s)",
                        StatusCodes.Status400BadRequest,
                        result.Error,
                        validationResult.Errors)),
                _ => result.ToProblemDetails()
            };

        private static ProblemDetails CreateProblemDetails(
            string title,
            int status,
            Error error,
            Error[]? errors = null) =>
            new()
            {
                Title = title,
                Type = error.Code,
                Detail = error.Description,
                Status = status,
                Extensions = { { nameof(errors), errors } }
            };

        public static IResult ToProblemDetails(this Result result)
        {
            if (result.IsSuccess)
            {
                throw new InvalidOperationException("Cannot create ProblemDetails from successful result");
            }

            return Results.Problem(
                statusCode: GetStatusCode(result.Error.Type),
                title: GetTitle(result.Error.Type),
                type: GetType(result.Error.Type),
                detail: result.Error.Description,
                extensions: new Dictionary<string, object?>
                {
                    { "errors", new[] { result.Error } }
                });

            static int GetStatusCode(ErrorType errorType) =>
                errorType switch
                {
                    ErrorType.Validation => StatusCodes.Status400BadRequest,
                    ErrorType.NotFound => StatusCodes.Status404NotFound,
                    ErrorType.Conflict => StatusCodes.Status409Conflict,
                    ErrorType.Unauthorized => StatusCodes.Status401Unauthorized,
                    ErrorType.None => 0,
                    _ => StatusCodes.Status500InternalServerError
                };

            static string GetTitle(ErrorType errorType) =>
                errorType switch
                {
                    ErrorType.Validation => "Bad Request",
                    ErrorType.NotFound => "Not Found",
                    ErrorType.Conflict => "Conflict",
                    ErrorType.Unauthorized => "Unauthorized",
                    ErrorType.None => "",
                    _ => "Server Failure"
                };

            static string GetType(ErrorType errorType) =>
                errorType switch
                {
                    ErrorType.Validation => "https://tools.ietf.org/html/rfc7231#section-6.5.1",
                    ErrorType.NotFound => "https://tools.ietf.org/html/rfc7231#section-6.5.4",
                    ErrorType.Conflict => "https://tools.ietf.org/html/rfc7231#section-6.5.8",
                    ErrorType.Unauthorized => "https://datatracker.ietf.org/doc/html/rfc7235#section-3.1",
                    ErrorType.None => "",
                    _ => "https://tools.ietf.org/html/rfc7231#section-6.6.1"
                };
        }
    }
}
