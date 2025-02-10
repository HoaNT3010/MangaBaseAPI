using MangaBaseAPI.Domain.Abstractions;
using MediatR;

namespace MangaBaseAPI.Application.Authentication.Commands.VerifyPasswordReset
{
    public record VerifyPasswordResetCommand(
        string Email,
        string NewPassword,
        string ConfirmNewPassword,
        string Token) : IRequest<Result<string>>;
}
