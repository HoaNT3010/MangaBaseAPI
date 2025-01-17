using MangaBaseAPI.Domain.Abstractions;
using MediatR;

namespace MangaBaseAPI.Application.Authentication.Commands.ResendEmailVerification
{
    public record ResendEmailVerificationCommand(
        string Email) : IRequest<Result>;
}
