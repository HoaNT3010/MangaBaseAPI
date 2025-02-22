using MangaBaseAPI.Domain.Abstractions;
using MangaBaseAPI.Domain.Constants.Caching;
using MangaBaseAPI.Domain.Entities;
using MangaBaseAPI.Domain.Errors.User;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;

namespace MangaBaseAPI.Application.Users.Commands.UpdateFullName
{
    public class UpdateUserFullNameCommandHandler(
        UserManager<User> userManager,
        IDistributedCache cache,
        ILogger<UpdateUserFullNameCommandHandler> logger)
        : IRequestHandler<UpdateUserFullNameCommand, Result>
    {
        public async Task<Result> Handle(
            UpdateUserFullNameCommand request,
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

            if (user.FirstName == request.FirstName.Trim() && user.LastName == request.LastName.Trim())
            {
                return Result.Failure(UserErrors.Update_UnchangedFullName);
            }
            user.FirstName = request.FirstName.Trim();
            user.LastName = request.LastName.Trim();
            user.SetModifyDateTime();
            try
            {
                await userManager.UpdateAsync(user);
            }
            catch (Exception e)
            {
                logger.LogError("Failed to update user's full name: {Message}", e.Message);
                return Result.Failure(UserErrors.Update_UpdatePersonalInformationFailed);
            }

            await cache.RefreshAsync(UserCachingConstants.GetByIdKey + request.Id, cancellationToken);
            return Result.SuccessNullError();
        }
    }
}
