using MangaBaseAPI.Contracts.Authentication.RevokeToken;
using MangaBaseAPI.Domain.Abstractions;
using MediatR;

namespace MangaBaseAPI.Application.Authentication.Commands.RevokeToken
{
    public record RevokeTokenCommand(
        string refreshToken) : IRequest<Result<RevokeTokenResponse>>;
}
