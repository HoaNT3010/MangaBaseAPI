using MangaBaseAPI.Contracts.Authentication.Login;
using MangaBaseAPI.CrossCuttingConcerns.Identity;
using MangaBaseAPI.CrossCuttingConcerns.Jwt;
using MangaBaseAPI.Domain.Abstractions;
using MangaBaseAPI.Domain.Entities;
using MangaBaseAPI.Domain.Errors.Authentication;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace MangaBaseAPI.Application.Authentication.Queries.Login
{
    public class LoginQueryHandler : IRequestHandler<LoginQuery, Result<LoginResponse>>
    {
        private readonly UserManager<User> _userManager;
        private readonly IPasswordHasher _passwordHasher;
        private readonly IJwtTokenProvider _jwtTokenProvider;

        public LoginQueryHandler(
            UserManager<User> userManager,
            IPasswordHasher passwordHasher,
            IJwtTokenProvider jwtTokenProvider)
        {
            _userManager = userManager;
            _passwordHasher = passwordHasher;
            _jwtTokenProvider = jwtTokenProvider;
        }

        public async Task<Result<LoginResponse>> Handle(LoginQuery request, CancellationToken cancellationToken)
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

            string accessToken = _jwtTokenProvider.GenerateAccessToken(user, new List<string>());
            string refreshToken = _jwtTokenProvider.GenerateRefreshToken();

            var loginResponse = new LoginResponse()
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken,
            };
            return Result.SuccessNullError(loginResponse);
        }
    }
}
