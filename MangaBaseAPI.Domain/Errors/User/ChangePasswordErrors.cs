using MangaBaseAPI.Domain.Abstractions;

namespace MangaBaseAPI.Domain.Errors.User
{
    public class ChangePasswordErrors
    {
        public static readonly Error UserNotFoundWithId = Error.NotFound("User.ChangePassword.UserNotFoundWithId", "No user found with the given ID");
        public static readonly Error OldPasswordReused = Error.Validation("User.ChangePassword.OldPasswordReused", "Old password cannot be reused");
        public static readonly Error ChangePasswordFailed = Error.Failure("User.ChangePassword.ChangePasswordFailed", "Unexpected error(s) occurred when trying to change user's password");
        public static readonly Error IncorrectCurrentPassword = Error.Validation("User.ChangePassword.IncorrectCurrentPassword", "Provided current password does not match user's current password");
    }
}
