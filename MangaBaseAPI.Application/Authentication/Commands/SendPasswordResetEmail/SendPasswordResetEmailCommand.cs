using MangaBaseAPI.Domain.Abstractions;
using MediatR;

namespace MangaBaseAPI.Application.Authentication.Commands.SendPasswordResetEmail
{
    public record SendPasswordResetEmailCommand(
        string Email) : IRequest<Result>;
}
