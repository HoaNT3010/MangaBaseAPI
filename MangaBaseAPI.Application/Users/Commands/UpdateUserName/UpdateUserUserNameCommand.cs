using MangaBaseAPI.Domain.Abstractions;
using MediatR;

namespace MangaBaseAPI.Application.Users.Commands.UpdateUserName
{
    public record UpdateUserUserNameCommand(
        Guid Id,
        string UserName) : IRequest<Result>;
}
