using MangaBaseAPI.Application.Authentication.Common.Specifications;
using MangaBaseAPI.Domain.Abstractions;
using MangaBaseAPI.Domain.Entities;
using MangaBaseAPI.Domain.Errors.Authentication;
using MangaBaseAPI.Domain.Repositories;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace MangaBaseAPI.Application.Authentication.Commands.VerifyEmail
{
    public class VerifyEmailCommandHandler
        : IRequestHandler<VerifyEmailCommand, Result<string>>
    {
        const string SuccessMessage = "Email verified successfully!";

        readonly UserManager<User> _userManager;
        readonly IUnitOfWork _unitOfWork;
        readonly ILogger<VerifyEmailCommandHandler> _logger;

        public VerifyEmailCommandHandler(
            UserManager<User> userManager,
            IUnitOfWork unitOfWork,
            ILogger<VerifyEmailCommandHandler> logger)
        {
            _userManager = userManager;
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<Result<string>> Handle(
            VerifyEmailCommand request,
            CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user == null)
            {
                return Result.Failure<string>(Error.ErrorWithValue(
                    EmailVerificationErrors.Verify_UserNotFoundWithEmail,
                    request.Email));
            }
            if (user!.EmailConfirmed)
            {
                return Result.Failure<string>(Error.ErrorWithValue(
                    EmailVerificationErrors.Verify_UserEmailVerified,
                    request.Email));
            }

            var userTokenRepository = _unitOfWork.GetRepository<IUserTokenRepository>();
            var verificationTokens = await userTokenRepository.ToListAsync(userTokenRepository.ApplySpecification(
                new GetUserEmailVerificationTokenSpecification(user.Id)), cancellationToken);

            var token = verificationTokens.FirstOrDefault(x => x.Value == request.Token);
            if (token == null)
            {
                return Result.Failure<string>(Error.ErrorWithValue(
                    EmailVerificationErrors.Verify_InvalidToken,
                    request.Token));
            }
            if (token.TokenExpiry < DateTimeOffset.UtcNow)
            {
                return Result.Failure<string>(Error.ErrorWithValue(
                    EmailVerificationErrors.Verify_TokenExpired,
                    request.Token));
            }
            try
            {
                await _unitOfWork.BeginTransactionAsync(cancellationToken);

                user.EmailConfirmed = true;
                await _userManager.UpdateAsync(user);
                userTokenRepository.BulkDelete(verificationTokens);
                await _unitOfWork.SaveChangeAsync(cancellationToken);

                await _unitOfWork.CommitTransactionAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackTransactionAsync(cancellationToken);
                _logger.LogError("Failed to verify user's email address: {Message}", ex.Message);
                return Result.Failure<string>(EmailVerificationErrors.Verify_VerifyEmailFailed);
            }

            return Result.SuccessNullError(SuccessMessage);
        }
    }
}
