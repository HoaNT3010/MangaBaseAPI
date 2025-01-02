using MangaBaseAPI.Domain.Abstractions;
using MediatR;

namespace MangaBaseAPI.Application.Titles.Commands.UpdateGenres
{
    public record UpdateTitleGenresCommand(
        Guid Id,
        List<int> Genres) : IRequest<Result>;
}
