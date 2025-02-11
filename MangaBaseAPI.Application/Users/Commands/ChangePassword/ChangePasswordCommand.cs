using MangaBaseAPI.Domain.Abstractions;
using MediatR;

namespace MangaBaseAPI.Application.Users.Commands.ChangePassword
{
    public record ChangePasswordCommand(
        string CurrentPassword,
        string NewPassword,
        string ConfirmNewPassword,
        Guid UserId) : IRequest<Result>;
}
