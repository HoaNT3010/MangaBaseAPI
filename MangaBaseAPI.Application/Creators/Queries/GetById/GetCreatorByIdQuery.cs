using MangaBaseAPI.Contracts.Creators.GetById;
using MangaBaseAPI.Domain.Abstractions;
using MediatR;

namespace MangaBaseAPI.Application.Creators.Queries.GetById
{
    public record GetCreatorByIdQuery(
        Guid Id) : IRequest<Result<GetCreatorByIdResponse>>;
}
