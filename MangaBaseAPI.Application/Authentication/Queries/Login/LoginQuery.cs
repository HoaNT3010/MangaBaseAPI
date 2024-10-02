using MangaBaseAPI.Domain.Entities;
using MediatR;

namespace MangaBaseAPI.Application.Authentication.Queries.Login
{
    public record LoginQuery(
        string Email,
        string Password) : IRequest<User?>;
}
