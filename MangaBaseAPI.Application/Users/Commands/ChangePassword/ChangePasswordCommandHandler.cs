using MangaBaseAPI.Application.Authentication.Common.Specifications;
using MangaBaseAPI.CrossCuttingConcerns.Identity;
using MangaBaseAPI.Domain.Abstractions;
using MangaBaseAPI.Domain.Entities;
using MangaBaseAPI.Domain.Errors.User;
using MangaBaseAPI.Domain.Repositories;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace MangaBaseAPI.Application.Users.Commands.ChangePassword
{
    public class ChangePasswordCommandHandler
        : IRequestHandler<ChangePasswordCommand, Result>
    {
        readonly IUnitOfWork _unitOfWork;
        readonly IPasswordHasher _passwordHasher;
        readonly UserManager<User> _userManager;
        readonly ILogger<ChangePasswordCommandHandler> _logger;

        public ChangePasswordCommandHandler(
            IUnitOfWork unitOfWork,
            IPasswordHasher passwordHasher,
            UserManager<User> userManager,
            ILogger<ChangePasswordCommandHandler> logger)
        {
            _unitOfWork = unitOfWork;
            _passwordHasher = passwordHasher;
            _userManager = userManager;
            _logger = logger;
        }

        public async Task<Result> Handle(
            ChangePasswordCommand request,
            CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByIdAsync(request.UserId.ToString());
            if (user == null)
            {
                return Result.Failure(Error.ErrorWithValue(
                    ChangePasswordErrors.UserNotFoundWithId,
                    request.UserId));
            }
            if (user.LockoutEnabled && user.LockoutEnd != null)
            {
                return Result.Failure(UserErrors.General_UserTimedOut);
            }
            if (!_passwordHasher.VerifyHashedPassword(user, user.PasswordHash!, request.CurrentPassword))
            {
                return Result.Failure(ChangePasswordErrors.IncorrectCurrentPassword);
            }

            var passwordHistoryRepository = _unitOfWork.GetRepository<IPasswordHistoryRepository>();
            var passwordHistories = await passwordHistoryRepository.ToListAsync(passwordHistoryRepository.ApplySpecification(
                new GetUserAllPasswordHistoriesSpecification(request.UserId)));

            if (passwordHistories != null && passwordHistories.Count > 0)
            {
                foreach (var passwordHistory in passwordHistories)
                {
                    if (_passwordHasher.VerifyHashedPassword(user, user.PasswordHash!, request.NewPassword))
                    {
                        return Result.Failure(ChangePasswordErrors.OldPasswordReused);
                    }
                }
            }
            try
            {
                await _unitOfWork.BeginTransactionAsync(cancellationToken);
                var newPasswordHash = _passwordHasher.HashProvidedPassword(user, request.NewPassword);
                var newPasswordHistory = new PasswordHistory()
                {
                    UserId = user.Id,
                    PasswordHash = newPasswordHash
                };
                user.PasswordHash = newPasswordHash;
                await passwordHistoryRepository.AddAsync(newPasswordHistory, cancellationToken);
                var updateResult = await _userManager.UpdateAsync(user);
                if (!updateResult.Succeeded)
                {
                    throw new InvalidOperationException($"Unexpected error(s) occurred when trying to save user's account after changing password: {string.Join(", ", updateResult.Errors)}");
                }
                await _unitOfWork.SaveChangeAsync(cancellationToken);
                await _unitOfWork.CommitTransactionAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackTransactionAsync(cancellationToken);
                _logger.LogError("Failed to change user's account password: {Message}", ex.Message);
                return Result.Failure(ChangePasswordErrors.ChangePasswordFailed);
            }

            return Result.SuccessNullError();
        }
    }
}
