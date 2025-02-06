using MangaBaseAPI.Application.Common.Utilities.Email;
using MangaBaseAPI.CrossCuttingConcerns.BackgroundJob.HangfireScheduler;
using MangaBaseAPI.CrossCuttingConcerns.Email.Gmail;
using MangaBaseAPI.CrossCuttingConcerns.Identity;
using MangaBaseAPI.Domain.Abstractions;
using MangaBaseAPI.Domain.Constants.Role;
using MangaBaseAPI.Domain.Constants.User;
using MangaBaseAPI.Domain.Entities;
using MangaBaseAPI.Domain.Errors.Authentication;
using MangaBaseAPI.Domain.Repositories;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace MangaBaseAPI.Application.Authentication.Commands.Register
{
    public class RegisterCommandHandler
        : IRequestHandler<RegisterCommand, Result>
    {
        private readonly UserManager<User> _userManager;
        private readonly IPasswordHasher _passwordHasher;
        private readonly IUnitOfWork _unitOfWork;
        readonly ILogger<RegisterCommandHandler> _logger;
        readonly IHangfireBackgroundJobService _jobService;

        public RegisterCommandHandler(
            UserManager<User> userManager,
            IPasswordHasher passwordHasher,
            IUnitOfWork unitOfWork,
            ILogger<RegisterCommandHandler> logger,
            IHangfireBackgroundJobService jobService)
        {
            _userManager = userManager;
            _passwordHasher = passwordHasher;
            _unitOfWork = unitOfWork;
            _logger = logger;
            _jobService = jobService;
        }

        public async Task<Result> Handle(
            RegisterCommand request,
            CancellationToken cancellationToken)
        {
            // Check if email is unique
            var isEmailUnique = await _userManager.FindByEmailAsync(request.Email);
            if (isEmailUnique != null)
            {
                return Result.Failure(RegisterErrors.EmailNotUnique);
            }

            // Check if user name is unique
            var isUserNameUnique = await _userManager.FindByNameAsync(request.UserName);
            if (isUserNameUnique != null)
            {
                return Result.Failure(RegisterErrors.UserNameNotUnique);
            }

            // Create user
            User newUser = new User(Guid.NewGuid(),
                request.UserName,
                request.Email);

            string hashedPassword = _passwordHasher.HashProvidedPassword(newUser, request.Password);
            newUser.SetInitialPassword(hashedPassword);

            try
            {
                await _unitOfWork.BeginTransactionAsync(cancellationToken);
                // Add user to db
                var addUserResult = await _userManager.CreateAsync(newUser);
                if (!addUserResult.Succeeded)
                {
                    await _unitOfWork.RollbackTransactionAsync(cancellationToken);
                    return Result.Failure(RegisterErrors.CreateUserFailed);
                }
                // Add user's role to db
                var addUserRoleResult = await _userManager.AddToRoleAsync(newUser, ApplicationRoles.Member);
                if (!addUserRoleResult.Succeeded)
                {
                    await _unitOfWork.RollbackTransactionAsync(cancellationToken);
                    return Result.Failure(RegisterErrors.AssignUserRoleFailed);
                }

                await _unitOfWork.CommitTransactionAsync(cancellationToken);
            }
            catch (Exception)
            {
                await _unitOfWork.RollbackTransactionAsync(cancellationToken);
                return Result.Failure(RegisterErrors.UnexpectedError);
            }

            // Add email verification process
            await CreateAndSendEmailVerification(newUser.Id, newUser.Email!, newUser.UserName!, cancellationToken);

            return Result.SuccessNullError();
        }

        private async Task CreateAndSendEmailVerification(
            Guid userId,
            string userEmail,
            string userDisplayName,
            CancellationToken cancellationToken = default)
        {
            var verificationToken = new UserToken()
            {
                UserId = userId,
                LoginProvider = UserTokenConstants.MangaBaseLoginProvider,
                Name = UserTokenConstants.EmailVerificationTokenName,
                Value = Guid.NewGuid().ToString(),
                TokenExpiry = DateTimeOffset.UtcNow.AddDays(1)
            };

            var userTokenRepository = _unitOfWork.GetRepository<IUserTokenRepository>();
            try
            {
                await userTokenRepository.AddAsync(verificationToken, cancellationToken);
                await _unitOfWork.SaveChangeAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError("Failed to create email verification token for user {UserId}: {Message}", userId, ex.Message);
            }

            _jobService.Enqueue<IGmailEmailService>(service => service.SendEmailAsync(
                userEmail,
                EmailHelper.EmailVerificationSubject,
                EmailHelper.GenerateEmailVerificationBody(userDisplayName, EmailHelper.GenerateEmailVerificationUrl_Development(userEmail, verificationToken.Value)),
                cancellationToken));
        }
    }
}
