using MangaBaseAPI.Domain.Abstractions;

namespace MangaBaseAPI.Domain.Errors.Authentication
{
    public static class LoginErrors
    {
        public static readonly Error InvalidCredentials = Error.Unauthorized("Login.InvalidCredentials", "Incorrect login credentials");
    }
}
