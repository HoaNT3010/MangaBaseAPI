using MangaBaseAPI.Contracts.Users.GetById;
using MangaBaseAPI.Domain.Abstractions;
using MediatR;

namespace MangaBaseAPI.Application.Users.Queries.GetById
{
    public record GetUserByIdQuery(
        Guid Id) : IRequest<Result<GetUserByIdResponse>>;
}
