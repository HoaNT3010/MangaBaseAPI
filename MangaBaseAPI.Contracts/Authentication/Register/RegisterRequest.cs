namespace MangaBaseAPI.Contracts.Authentication.Register
{
    public record RegisterRequest(
        string UserName,
        string Email,
        string Password,
        string ConfirmedPassword);
}
