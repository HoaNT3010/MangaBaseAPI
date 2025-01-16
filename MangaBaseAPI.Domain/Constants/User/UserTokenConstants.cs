namespace MangaBaseAPI.Domain.Constants.User
{
    public class UserTokenConstants
    {
        #region Providers
        public const string MangaBaseLoginProvider = "MangaBase";
        #endregion

        #region TokenNames
        public const string JwtRefreshTokenName = "JwtRefreshToken";
        public const string EmailVerificationTokenName = "EmailVerification";
        public const string PasswordResetTokenName = "PasswordReset";
        #endregion
    }
}
