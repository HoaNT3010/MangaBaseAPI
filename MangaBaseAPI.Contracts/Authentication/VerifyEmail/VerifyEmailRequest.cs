namespace MangaBaseAPI.Contracts.Authentication.VerifyEmail
{
    public record VerifyEmailRequest(
        string Email,
        string Token);
}
