using MangaBaseAPI.Domain.Constants.Location;

namespace MangaBaseAPI.Application.Common.Utilities.Email
{
    public class EmailHelper
    {
        public const string ApplicationName = "MangaBase";

        #region EmailVerification
        public const string EmailVerificationSubject = $"Verify your email for {ApplicationName}";
        const string EmailParameterName = "email";
        const string VerificationTokenParameterName = "token";

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
    }
}
