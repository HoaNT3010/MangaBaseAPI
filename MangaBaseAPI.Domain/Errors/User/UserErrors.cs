﻿using MangaBaseAPI.Domain.Abstractions;

namespace MangaBaseAPI.Domain.Errors.User
{
    public class UserErrors
    {
        #region General
        public static readonly Error General_UserTimedOut = Error.Forbidden("User.General.UserTimedOut", "User has been timed out and has restricted access");
        public static readonly Error General_UserNotFound = Error.NotFound("User.General.UserNotFound", "User cannot be found");
        public static readonly Error General_UserNotFoundWithId = Error.NotFound("User.General.UserNotFoundWithId", "No user found with the given ID");
        public static readonly Error General_UserNotFoundWithEmail = Error.NotFound("User.General.UserNotFoundWithEmail", "No user found with the given email");
        #endregion
    }
}
