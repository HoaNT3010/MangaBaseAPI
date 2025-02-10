namespace MangaBaseAPI.Contracts.Authentication.VerifyPasswordReset
{
    public record VerifyPasswordResetRequest(
        string Email,
        string NewPassword,
        string ConfirmNewPassword,
        string Token);
}
