using MangaBaseAPI.Domain.Abstractions;
using MediatR;

namespace MangaBaseAPI.Application.Users.Commands.UpdateFullName
{
    public record UpdateUserFullNameCommand(
        Guid Id,
        string FirstName,
        string LastName) : IRequest<Result>;
}
