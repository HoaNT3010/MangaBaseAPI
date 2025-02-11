using MangaBaseAPI.Application.Authentication.Common.Specifications;
using MangaBaseAPI.Application.Common.Specifications;
using MangaBaseAPI.CrossCuttingConcerns.Identity;
using MangaBaseAPI.Domain.Abstractions;
using MangaBaseAPI.Domain.Entities;
using MangaBaseAPI.Domain.Errors.Authentication;
using MangaBaseAPI.Domain.Repositories;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace MangaBaseAPI.Application.Authentication.Commands.VerifyPasswordReset
{
    public class VerifyPasswordResetCommandHandler
        : IRequestHandler<VerifyPasswordResetCommand, Result<string>>
    {
        const string SuccessMessage = "Password reset successfully!";

        readonly IUnitOfWork _unitOfWork;
        readonly UserManager<User> _userManager;
        readonly ILogger<VerifyPasswordResetCommand> _logger;
        readonly IPasswordHasher _passwordHasher;

        public VerifyPasswordResetCommandHandler(
            IUnitOfWork unitOfWork,
            UserManager<User> userManager,
            ILogger<VerifyPasswordResetCommand> logger,
            IPasswordHasher passwordHasher)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
            _logger = logger;
            _passwordHasher = passwordHasher;
        }

        public async Task<Result<string>> Handle(
            VerifyPasswordResetCommand request,
            CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user == null)
            {
                return Result.Failure<string>(Error.ErrorWithValue(
                    PasswordResetErrors.Reset_UserNotFoundWithEmail,
                    request.Email));
            }

            var userTokenRepository = _unitOfWork.GetRepository<IUserTokenRepository>();

            // Validate reset token
            var resetTokens = await userTokenRepository.ToListAsync(userTokenRepository.ApplySpecification(
                new GetPasswordResetTokenSpecification(user.Id)), cancellationToken);
            var token = resetTokens.FirstOrDefault(x => x.Value == request.Token);
            if (token == null)
            {
                return Result.Failure<string>(Error.ErrorWithValue(
                    PasswordResetErrors.Reset_InvalidToken,
                    request.Token));
            }
            if (token.TokenExpiry < DateTimeOffset.UtcNow)
            {
                return Result.Failure<string>(Error.ErrorWithValue(
                    PasswordResetErrors.Reset_TokenExpired,
                    request.Token));
            }

            // Check for old password reuse
            var passwordHistoryRepository = _unitOfWork.GetRepository<IPasswordHistoryRepository>();
            var passwordHistories = await passwordHistoryRepository.ToListAsync(passwordHistoryRepository.ApplySpecification(
                new GetUserAllPasswordHistoriesSpecification(user.Id)), cancellationToken);
            if (passwordHistories != null && passwordHistories.Count > 0)
            {
                foreach (var passwordHistory in passwordHistories)
                {
                    if (_passwordHasher.VerifyHashedPassword(user, passwordHistory.PasswordHash!, request.NewPassword))
                    {
                        return Result.Failure<string>(PasswordResetErrors.Reset_OldPasswordReused);
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
                    throw new InvalidOperationException($"Unexpected error(s) occurred when trying to save user's account after password reset: {string.Join(", ", updateResult.Errors)}");
                }
                userTokenRepository.BulkDelete(resetTokens);

                await _unitOfWork.SaveChangeAsync(cancellationToken);
                await _unitOfWork.CommitTransactionAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackTransactionAsync(cancellationToken);
                _logger.LogError("Failed to verify user's password reset operation: {Message}", ex.Message);
                return Result.Failure<string>(PasswordResetErrors.Reset_ResetPasswordFailed);
            }
            return Result.SuccessNullError(SuccessMessage);
        }
    }
}
