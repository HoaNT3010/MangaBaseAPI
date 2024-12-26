using MangaBaseAPI.Contracts.Titles.GetById;
using MangaBaseAPI.Domain.Abstractions;
using MediatR;

namespace MangaBaseAPI.Application.Tittles.Queries.GetById
{
    public record GetTitleByIdQuery(
        Guid Id) : IRequest<Result<GetTitleByIdResponse>>;
}
