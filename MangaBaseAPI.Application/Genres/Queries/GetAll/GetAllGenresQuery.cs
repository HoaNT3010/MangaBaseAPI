using MangaBaseAPI.Contracts.Genres.GetAll;
using MangaBaseAPI.Domain.Abstractions;
using MangaBaseAPI.Domain.Entities;
using MediatR;

namespace MangaBaseAPI.Application.Genres.Queries.GetAll
{
    public record GetAllGenresQuery()
        : IRequest<Result<List<GenreResponse>>>;
}
