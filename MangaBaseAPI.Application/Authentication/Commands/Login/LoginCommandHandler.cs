using MangaBaseAPI.Contracts.Authentication.Login;
using MangaBaseAPI.CrossCuttingConcerns.Identity;
using MangaBaseAPI.CrossCuttingConcerns.Jwt;
using MangaBaseAPI.Domain.Abstractions;
using MangaBaseAPI.Domain.Constants.User;
using MangaBaseAPI.Domain.Entities;
using MangaBaseAPI.Domain.Errors.Authentication;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace MangaBaseAPI.Application.Authentication.Commands.Login
{
    public class LoginCommandHandler : IRequestHandler<LoginCommand, Result<LoginResponse>>
    {
        private readonly UserManager<User> _userManager;
        private readonly IPasswordHasher _passwordHasher;
        private readonly IJwtTokenProvider _jwtTokenProvider;

        public LoginCommandHandler(
            UserManager<User> userManager,
            IPasswordHasher passwordHasher,
            IJwtTokenProvider jwtTokenProvider)
        {
            _userManager = userManager;
            _passwordHasher = passwordHasher;
            _jwtTokenProvider = jwtTokenProvider;
        }

        public async Task<Result<LoginResponse>> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user is null)
            {
                return Result.Failure<LoginResponse>(LoginErrors.InvalidCredentials);
            }

            bool isCorrectPassword = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash!, request.Password);
            if (!isCorrectPassword)
            {
                return Result.Failure<LoginResponse>(LoginErrors.InvalidCredentials);
            }

            // Update user's refresh token
            IList<string> roles = await _userManager.GetRolesAsync(user);
            string refreshToken = _jwtTokenProvider.GenerateRefreshToken();
            string accessToken = _jwtTokenProvider.GenerateAccessToken(user, roles);

            var tokenUpdate = await _userManager.SetAuthenticationTokenAsync(user, UserTokenConstants.MangaBaseLoginProvider, UserTokenConstants.JwtRefreshTokenName, refreshToken);

            if (!tokenUpdate.Succeeded)
            {
                return Result.Failure<LoginResponse>(LoginErrors.UpdateRefreshTokenFailed);
            }

            var loginResponse = new LoginResponse()
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken,
            };

            return Result.SuccessNullError(loginResponse);
        }
    }
}
