using MangaBaseAPI.Contracts.Authentication.RefreshToken;
using MangaBaseAPI.Domain.Abstractions;
using MediatR;

namespace MangaBaseAPI.Application.Authentication.Commands.RefreshToken
{
    public record RefreshTokenCommand(
        string refreshToken) : IRequest<Result<RefreshTokenResponse>>;
}
