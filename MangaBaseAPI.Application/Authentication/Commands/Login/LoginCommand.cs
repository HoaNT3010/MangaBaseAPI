using MangaBaseAPI.Contracts.Authentication.Login;
using MangaBaseAPI.Domain.Abstractions;
using MediatR;

namespace MangaBaseAPI.Application.Authentication.Commands.Login
{
    public record LoginCommand(
        string Email,
        string Password) : IRequest<Result<LoginResponse>>;
}
