using MangaBaseAPI.Contracts.Authentication.RevokeToken;
using MangaBaseAPI.CrossCuttingConcerns.Jwt;
using MangaBaseAPI.Domain.Abstractions;
using MangaBaseAPI.Domain.Errors.Authentication;
using MangaBaseAPI.Domain.Repositories;
using MediatR;

namespace MangaBaseAPI.Application.Authentication.Commands.RevokeToken
{
    public class RevokeTokenCommandHandler : IRequestHandler<
        RevokeTokenCommand,
        Result<RevokeTokenResponse>>
    {
        const string SuccessResponseMessage = "Token revoked successfully";

        private readonly IJwtTokenProvider _jwtTokenProvider;
        private readonly IUnitOfWork _unitOfWork;

        public RevokeTokenCommandHandler(
            IJwtTokenProvider jwtTokenProvider,
            IUnitOfWork unitOfWork)
        {
            _jwtTokenProvider = jwtTokenProvider;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<RevokeTokenResponse>> Handle(
            RevokeTokenCommand request,
            CancellationToken cancellationToken)
        {
            if (_jwtTokenProvider.IsTokenInvalidOrExpired(request.refreshToken))
            {
                return Result.Failure<RevokeTokenResponse>(RefreshTokenErrors.InvalidOrExpiredToken);
            }

            bool removeTokenResult = await _unitOfWork
                .GetRepository<IUserTokenRepository>()
                .TryRemoveTokenByValueAsync(request.refreshToken, cancellationToken);

            if (!removeTokenResult)
            {
                return Result.Failure<RevokeTokenResponse>(RefreshTokenErrors.TokenEntryNotFound);
            }

            int updateResult = await _unitOfWork.SaveChangeAsync();
            if (updateResult <= 0)
            {
                return Result.Failure<RevokeTokenResponse>(RefreshTokenErrors.UpdateRefreshTokenFailed);
            }

            return Result.SuccessNullError(new RevokeTokenResponse(SuccessResponseMessage));
        }
    }
}
