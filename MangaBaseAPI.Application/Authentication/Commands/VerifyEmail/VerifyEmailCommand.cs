using MangaBaseAPI.Domain.Abstractions;
using MediatR;

namespace MangaBaseAPI.Application.Authentication.Commands.VerifyEmail
{
    public record VerifyEmailCommand(
        string Email,
        string Token) : IRequest<Result<string>>;
}
