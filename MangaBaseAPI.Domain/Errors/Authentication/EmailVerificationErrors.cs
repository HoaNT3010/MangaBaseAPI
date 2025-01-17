using MangaBaseAPI.Domain.Abstractions;

namespace MangaBaseAPI.Domain.Errors.Authentication
{
    public class EmailVerificationErrors
    {
        #region Resend
        public static readonly Error Resend_UserNotFoundWithEmail = Error.NotFound("EmailVerification.Resend.UserNotFoundWithEmail", "No user found with the given email");
        public static readonly Error Resend_UserEmailVerified = Error.Validation("EmailVerification.Resend.UserEmailVerified", "The email address has been verified");
        public static readonly Error Resend_SendVerificationEmailFailed = Error.Failure("EmailVerification.Resend.ResendVerificationEmailFailed", "Unexpected error(s) occurred when trying to send verification email to user");
        public static readonly Error Resend_GenerateNewTokenFailed = Error.Failure("EmailVerification.Resend.GenerateNewTokenFailed", "Unexpected error(s) occurred when trying to generate new token for email verification");
        #endregion

        #region Verify
        public static readonly Error Verify_UserNotFoundWithEmail = Error.NotFound("EmailVerification.Verify.UserNotFoundWithEmail", "No user found with the given email");
        public static readonly Error Verify_UserEmailVerified = Error.Validation("EmailVerification.Verify.UserEmailVerified", "The email address has been verified");
        public static readonly Error Verify_VerifyEmailFailed = Error.Failure("EmailVerification.Verify.VerifyEmailFailed", "Unexpected error(s) occurred when trying to verify user's email");
        public static readonly Error Verify_InvalidToken = Error.Validation("EmailVerification.Verify.InvalidToken", "The verification token is invalid and cannot be used to verify user's email");
        public static readonly Error Verify_TokenExpired = Error.Validation("EmailVerification.Verify.TokenExpired", "The verification token is expired");
        #endregion

    }
}
