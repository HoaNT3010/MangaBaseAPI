using MangaBaseAPI.Contracts.Authentication.Login;
using MangaBaseAPI.Domain.Abstractions;
using MangaBaseAPI.Domain.Entities;
using MediatR;

namespace MangaBaseAPI.Application.Authentication.Queries.Login
{
    public record LoginQuery(
        string Email,
        string Password) : IRequest<Result<LoginResponse>>;
}
