using MangaBaseAPI.Domain.Abstractions;
using MediatR;

namespace MangaBaseAPI.Application.Authentication.Commands.Register
{
    public record RegisterCommand(
        string UserName,
        string Email,
        string Password,
        string ConfirmedPassword) : IRequest<Result>;
}
