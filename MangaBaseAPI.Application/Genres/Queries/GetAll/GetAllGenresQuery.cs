using MangaBaseAPI.Contracts.Genres.GetAll;
using MangaBaseAPI.Domain.Abstractions;
using MediatR;

namespace MangaBaseAPI.Application.Genres.Queries.GetAll
{
    public record GetAllGenresQuery()
        : IRequest<Result<List<GenreResponse>>>;
}
