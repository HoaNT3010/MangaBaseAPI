using MangaBaseAPI.Domain.Abstractions;
using MediatR;

namespace MangaBaseAPI.Application.Titles.Commands.UpdateArtists
{
    public record UpdateTitleArtistsCommand(
        Guid Id,
        List<Guid> Artists) : IRequest<Result>;
}
