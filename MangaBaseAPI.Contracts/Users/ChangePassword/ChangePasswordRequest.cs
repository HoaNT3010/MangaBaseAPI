namespace MangaBaseAPI.Contracts.Users.ChangePassword
{
    public record ChangePasswordRequest(
        string CurrentPassword,
        string NewPassword,
        string ConfirmNewPassword);
}
