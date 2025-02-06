using MangaBaseAPI.Application.Authentication.Common.Specifications;
using MangaBaseAPI.Application.Common.Utilities.Email;
using MangaBaseAPI.CrossCuttingConcerns.Email.Gmail;
using MangaBaseAPI.Domain.Abstractions;
using MangaBaseAPI.Domain.Constants.User;
using MangaBaseAPI.Domain.Entities;
using MangaBaseAPI.Domain.Errors.Authentication;
using MangaBaseAPI.Domain.Repositories;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace MangaBaseAPI.Application.Authentication.Commands.SendPasswordResetEmail
{
    public class SendPasswordResetEmailCommandHandler
        : IRequestHandler<SendPasswordResetEmailCommand, Result>
    {
        readonly IUnitOfWork _unitOfWork;
        readonly IGmailEmailService _emailService;
        readonly UserManager<User> _userManager;
        readonly ILogger<SendPasswordResetEmailCommandHandler> _logger;

        public SendPasswordResetEmailCommandHandler(
            IUnitOfWork unitOfWork,
            IGmailEmailService emailService,
            UserManager<User> userManager,
            ILogger<SendPasswordResetEmailCommandHandler> logger)
        {
            _unitOfWork = unitOfWork;
            _emailService = emailService;
            _userManager = userManager;
            _logger = logger;
        }

        public async Task<Result> Handle(
            SendPasswordResetEmailCommand request,
            CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user == null)
            {
                return Result.Failure(Error.ErrorWithValue(
                    PasswordResetErrors.Send_UserNotFoundWithEmail,
                    request.Email));
            }

            var userTokenRepository = _unitOfWork.GetRepository<IUserTokenRepository>();
            var passwordResetTokens = await userTokenRepository.ToListAsync(userTokenRepository.ApplySpecification(
                new GetPasswordResetTokenSpecification(user.Id)), cancellationToken);

            var resetToken = new UserToken()
            {
                UserId = user.Id,
                LoginProvider = UserTokenConstants.MangaBaseLoginProvider,
                Name = UserTokenConstants.PasswordResetTokenName,
                Value = Guid.NewGuid().ToString(),
                TokenExpiry = DateTimeOffset.UtcNow.AddHours(1)
            };

            try
            {
                if (passwordResetTokens != null && passwordResetTokens.Count > 0)
                {
                    userTokenRepository.BulkDelete(passwordResetTokens);
                }

                await userTokenRepository.AddAsync(resetToken, cancellationToken);
                await _unitOfWork.SaveChangeAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError("Failed to clean up and create new password reset token for user with email {Email}: {Message}", request.Email, ex.Message);
                return Result.Failure(PasswordResetErrors.Send_GenerateResetTokenFailed);
            }

            try
            {
                await _emailService.SendEmailAsync(
                    request.Email,
                    EmailHelper.PasswordResetSubject,
                    EmailHelper.GeneratePasswordResetEmailBody(user.UserName!, EmailHelper.GeneratePasswordResetUrl_Development(request.Email, resetToken.Value)),
                    cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError("Failed to send email contains password reset url to {Email}: {Message}", request.Email, ex.Message);
                return Result.Failure(PasswordResetErrors.Send_SendPasswordResetEmailFailed);
            }
            return Result.SuccessNullError();
        }
    }
}
