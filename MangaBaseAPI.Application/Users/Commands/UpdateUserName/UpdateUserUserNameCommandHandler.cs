using MangaBaseAPI.Domain.Abstractions;
using MangaBaseAPI.Domain.Constants.Caching;
using MangaBaseAPI.Domain.Entities;
using MangaBaseAPI.Domain.Errors.User;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;

namespace MangaBaseAPI.Application.Users.Commands.UpdateUserName
{
    public class UpdateUserUserNameCommandHandler(
        UserManager<User> userManager,
        IDistributedCache cache,
        ILogger<UpdateUserUserNameCommandHandler> logger)
        : IRequestHandler<UpdateUserUserNameCommand, Result>
    {
        public async Task<Result> Handle(
            UpdateUserUserNameCommand request,
            CancellationToken cancellationToken)
        {
            var user = await userManager.FindByIdAsync(request.Id.ToString());
            if (user == null)
            {
                return Result.Failure(UserErrors.General_UserNotFound);
            }
            if (user.IsUserCurrentlyLockedOut())
            {
                return Result.Failure(Error.ErrorWithValue(
                    UserErrors.General_UserTimedOut,
                    user.LockoutEnd));
            }

            var trimmedUserName = request.UserName.Trim();
            var normalizeUserName = userManager.NormalizeName(trimmedUserName);
            if (user.NormalizedUserName == normalizeUserName)
            {
                return Result.Failure(Error.ErrorWithValue(
                    UserErrors.Update_UnchangedUserName, trimmedUserName));
            }
            var existingUser = await userManager.FindByNameAsync(trimmedUserName);
            if (existingUser != null && existingUser.Id != user.Id)
            {
                return Result.Failure(Error.ErrorWithValue(
                    UserErrors.Update_UserNameNotUnique,
                    trimmedUserName));
            }

            user.UserName = trimmedUserName;
            user.NormalizedUserName = normalizeUserName;
            user.SetModifyDateTime();
            var updateResult = await userManager.UpdateAsync(user);
            if (!updateResult.Succeeded)
            {
                foreach (var error in updateResult.Errors)
                {
                    logger.LogError("Failed to update user's username: {Code} - {Description}", error.Code, error.Description);
                }
                return Result.Failure(UserErrors.Update_UpdatePersonalInformationFailed);
            }
            await cache.RemoveAsync(UserCachingConstants.GetByIdKey + request.Id, cancellationToken);
            return Result.SuccessNullError();
        }
    }
}
