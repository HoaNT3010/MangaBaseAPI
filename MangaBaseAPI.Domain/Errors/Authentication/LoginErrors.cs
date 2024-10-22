using MangaBaseAPI.Domain.Abstractions;

namespace MangaBaseAPI.Domain.Errors.Authentication
{
    public static class LoginErrors
    {
        public static readonly Error InvalidCredentials = Error.Unauthorized("Login.InvalidCredentials", "Incorrect login credentials");
        public static readonly Error UpdateRefreshTokenFailed = Error.Failure("Login.UpdateRefreshTokenFailed", "Failed to update user's refresh token");
    }
}
