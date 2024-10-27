using MangaBaseAPI.Domain.Abstractions;

namespace MangaBaseAPI.Domain.Errors.Authentication
{
    public static class RefreshTokenErrors
    {
        public static readonly Error InvalidOrExpiredToken = Error.Validation("RefreshToken.InvalidOrExpiredToken", "The provided token is invalid or expired");
        public static readonly Error UserNotFound = Error.NotFound("RefreshToken.UserNotFound", "No user found with the associated token");
        public static readonly Error UpdateRefreshTokenFailed = Error.Failure("RefreshToken.UpdateRefreshTokenFailed", "Failed to update user's refresh token");
    }
}
