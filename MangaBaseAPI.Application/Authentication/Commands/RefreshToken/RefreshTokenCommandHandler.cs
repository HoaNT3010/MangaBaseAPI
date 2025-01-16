using MangaBaseAPI.Contracts.Authentication.RefreshToken;
using MangaBaseAPI.CrossCuttingConcerns.Jwt;
using MangaBaseAPI.Domain.Abstractions;
using MangaBaseAPI.Domain.Constants.User;
using MangaBaseAPI.Domain.Entities;
using MangaBaseAPI.Domain.Errors.Authentication;
using MangaBaseAPI.Domain.Repositories;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace MangaBaseAPI.Application.Authentication.Commands.RefreshToken
{
    public class RefreshTokenCommandHandler : IRequestHandler<RefreshTokenCommand, Result<RefreshTokenResponse>>
    {
        private readonly UserManager<User> _userManager;
        private readonly IJwtTokenProvider _jwtTokenProvider;
        private readonly IUnitOfWork _unitOfWork;

        public RefreshTokenCommandHandler(
            UserManager<User> userManager,
            IJwtTokenProvider jwtTokenProvider,
            IUnitOfWork unitOfWork)
        {
            _userManager = userManager;
            _jwtTokenProvider = jwtTokenProvider;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<RefreshTokenResponse>> Handle(
            RefreshTokenCommand request,
            CancellationToken cancellationToken)
        {
            bool isTokenExpired = _jwtTokenProvider.IsTokenInvalidOrExpired(request.refreshToken);
            if (isTokenExpired)
            {
                return Result.Failure<RefreshTokenResponse>(RefreshTokenErrors.InvalidOrExpiredToken);
            }

            var userTokenRepository = _unitOfWork.GetRepository<IUserTokenRepository>();
            var userId = await userTokenRepository.GetUserIdByTokenValue(request.refreshToken);
            if (userId == default)
            {
                return Result.Failure<RefreshTokenResponse>(RefreshTokenErrors.UserNotFound);
            }

            var user = await _userManager.FindByIdAsync(userId.ToString());
            if (user == default)
            {
                return Result.Failure<RefreshTokenResponse>(RefreshTokenErrors.UserNotFound);
            }

            string refreshToken = _jwtTokenProvider.GenerateRefreshToken();
            string accessToken = _jwtTokenProvider.GenerateAccessToken(user, new List<string>());

            var tokenUpdateOperation = await _userManager.SetAuthenticationTokenAsync(user, UserTokenConstants.MangaBaseLoginProvider, UserTokenConstants.JwtRefreshTokenName, refreshToken);

            if (!tokenUpdateOperation.Succeeded)
            {
                return Result.Failure<RefreshTokenResponse>(RefreshTokenErrors.UpdateRefreshTokenFailed);
            }

            var response = new RefreshTokenResponse()
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken,
            };

            return Result.SuccessNullError(response);
        }
    }
}
