using MangaBaseAPI.Domain.Abstractions;
using MediatR;

namespace MangaBaseAPI.Application.Tittles.Commands.UpdateGenres
{
    public record UpdateTitleGenresCommand(
        Guid Id,
        List<int> Genres) : IRequest<Result>;
}
