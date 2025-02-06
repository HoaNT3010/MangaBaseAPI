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

namespace MangaBaseAPI.Application.Authentication.Commands.ResendEmailVerification
{
    public class ResendEmailVerificationCommandHandler
        : IRequestHandler<ResendEmailVerificationCommand, Result>
    {
        readonly IUnitOfWork _unitOfWork;
        readonly IGmailEmailService _emailService;
        readonly UserManager<User> _userManager;
        readonly ILogger<ResendEmailVerificationCommandHandler> _logger;

        public ResendEmailVerificationCommandHandler(
            IUnitOfWork unitOfWork,
            IGmailEmailService emailService,
            UserManager<User> userManager,
            ILogger<ResendEmailVerificationCommandHandler> logger)
        {
            _unitOfWork = unitOfWork;
            _emailService = emailService;
            _userManager = userManager;
            _logger = logger;
        }

        public async Task<Result> Handle(
            ResendEmailVerificationCommand request,
            CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user == null)
            {
                return Result.Failure(Error.ErrorWithValue(
                    EmailVerificationErrors.Resend_UserNotFoundWithEmail,
                    request.Email));
            }
            if (user!.EmailConfirmed)
            {
                return Result.Failure(Error.ErrorWithValue(
                    EmailVerificationErrors.Resend_UserEmailVerified,
                    request.Email));
            }

            var userTokenRepository = _unitOfWork.GetRepository<IUserTokenRepository>();
            var emailVerificationTokens = await userTokenRepository.ToListAsync(userTokenRepository.ApplySpecification(
                new GetUserEmailVerificationTokenSpecification(user.Id)), cancellationToken);

            var verificationToken = new UserToken()
            {
                UserId = user.Id,
                LoginProvider = UserTokenConstants.MangaBaseLoginProvider,
                Name = UserTokenConstants.EmailVerificationTokenName,
                Value = Guid.NewGuid().ToString(),
                TokenExpiry = DateTimeOffset.UtcNow.AddDays(1)
            };

            try
            {
                if (emailVerificationTokens != null && emailVerificationTokens.Count > 0)
                {
                    userTokenRepository.BulkDelete(emailVerificationTokens);
                }

                await userTokenRepository.AddAsync(verificationToken, cancellationToken);
                await _unitOfWork.SaveChangeAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError("Failed to clean up and create new email verification token for email {Email}: {Message}", request.Email, ex.Message);
                return Result.Failure(EmailVerificationErrors.Resend_GenerateNewTokenFailed);
            }

            try
            {
                await _emailService.SendEmailAsync(
                    request.Email,
                    EmailHelper.EmailVerificationSubject,
                    EmailHelper.GenerateEmailVerificationBody(user.UserName!, EmailHelper.GenerateEmailVerificationUrl_Development(request.Email, verificationToken.Value)),
                    cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError("Failed to send email with new email verification url to {Email}: {Message}", request.Email, ex.Message);
                return Result.Failure(EmailVerificationErrors.Resend_SendVerificationEmailFailed);
            }

            return Result.SuccessNullError();
        }
    }
}
