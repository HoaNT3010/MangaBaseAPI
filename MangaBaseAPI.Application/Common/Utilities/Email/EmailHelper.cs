using MangaBaseAPI.Domain.Constants.Location;

namespace MangaBaseAPI.Application.Common.Utilities.Email
{
    /// <summary>
    /// All url generation methods is currently using the API domain.
    /// This is a temporary solution for development environment.
    /// In production, the domain should be the application/frontend domain.
    /// </summary>
    public class EmailHelper
    {
        public const string ApplicationName = "MangaBase";

        #region EmailVerification
        public const string EmailVerificationSubject = $"Verify your email for {ApplicationName}";
        const string EmailParameterName = "email";
        const string VerificationTokenParameterName = "token";
        const string TokenParameterName = "token";

        public static string GenerateEmailVerificationBody(string displayName, string verificationUrl)
        {
            return $"Hello {displayName}, " +
                $"Follow this link to verify your email address. " +
                $"{verificationUrl} " +
                $"This link will expire in 24 hours. " +
                $"If you did not ask to verify this address, you can ignore this email. " +
                $"Thanks, " +
                $"{ApplicationName} team.";
        }

        /// <summary>
        /// <b>ONLY USE IN DEVELOPMENT ENVIRONMENT</b> Temporary method for generating email verification url.
        /// </summary>
        /// <param name="email"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public static string GenerateEmailVerificationUrl_Development(string email, string token)
        {
            string queryParams = $"?{EmailParameterName}={email}&{VerificationTokenParameterName}={token}";
            return $"{LocationConstants.ApplicationDevelopmentHttpsRoot}{LocationConstants.ApiV1BaseLocation}{LocationConstants.AuthenticationResource}email/verify{queryParams}";
        }
        #endregion

        #region PasswordReset
        public const string PasswordResetSubject = $"Reset your account's password for {ApplicationName}";

        public static string GeneratePasswordResetEmailBody(string displayName, string passwordResetUrl)
        {
            return $"Hello {displayName}, " +
                $"Follow this link to reset password for your account: " +
                $"{passwordResetUrl} " +
                $"This link will expire in 1 hour. " +
                $"If you did not ask to reset your account password, you can ignore this email. " +
                $"Thanks, " +
                $"{ApplicationName} team.";
        }

        public static string GeneratePasswordResetUrl_Development(string email, string token)
        {
            string queryParams = $"?{EmailParameterName}={email}&{TokenParameterName}={token}";
            return $"{LocationConstants.ApplicationDevelopmentHttpsRoot}{LocationConstants.ApiV1BaseLocation}{LocationConstants.AuthenticationResource}password/reset{queryParams}";
        }
        #endregion
    }
}
