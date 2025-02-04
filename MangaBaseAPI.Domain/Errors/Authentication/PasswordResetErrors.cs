using MangaBaseAPI.Domain.Abstractions;

namespace MangaBaseAPI.Domain.Errors.Authentication
{
    public class PasswordResetErrors
    {
        #region SendEmail
        public static readonly Error Send_UserNotFoundWithEmail = Error.NotFound("PasswordReset.Send.UserNotFoundWithEmail", "No user found with the given email");
        public static readonly Error Send_SendPasswordResetEmailFailed = Error.Failure("PasswordReset.Send.SendPasswordResetEmailFailed", "Unexpected error(s) occurred when trying to send password reset email to user");
        public static readonly Error Send_GenerateResetTokenFailed = Error.Failure("PasswordReset.Send.GenerateNewTokenFailed", "Unexpected error(s) occurred when trying to generate new token for password reset");
        #endregion

        #region Reset
        public static readonly Error Reset_UserNotFoundWithEmail = Error.NotFound("PasswordReset.Reset.UserNotFoundWithEmail", "No user found with the given email");
        public static readonly Error Reset_ResetPasswordFailed = Error.Failure("PasswordReset.Reset.ResetPasswordFailed", "Unexpected error(s) occurred when trying to reset user's account password");
        public static readonly Error Reset_InvalidToken = Error.Validation("PasswordReset.Reset.InvalidToken", "The password reset token is invalid and cannot be used to reset user's account password");
        public static readonly Error Reset_TokenExpired = Error.Validation("PasswordReset.Reset.TokenExpired", "The password reset token is expired");
        public static readonly Error Reset_OldPasswordReused = Error.Validation("PasswordReset.Reset.OldPasswordReused", "Old password cannot be reused");
        #endregion
    }
}
