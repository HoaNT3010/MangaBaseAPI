using MangaBaseAPI.Domain.Abstractions;

namespace MangaBaseAPI.Domain.Errors.Authentication
{
    public class RegisterErrors
    {
        public static readonly Error EmailNotUnique = Error.Conflict("Register.EmailNotUnique", "Email address has been used for account registration");
        public static readonly Error UserNameNotUnique = Error.Conflict("Register.UserNameNotUnique", "User name has been used");
        public static readonly Error CreateUserFailed = Error.Failure("Register.CreateUserFailed", "Failed to create new user account");
        public static readonly Error AssignUserRoleFailed = Error.Failure("Register.AssignUserRoleFailed", "Failed to create new user account");
        public static readonly Error UnexpectedError = Error.Failure("Register.UnexpectedError", "Unexpected error(s) occurred when trying to create new user account");
    }
}
