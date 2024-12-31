using MangaBaseAPI.Domain.Abstractions;
using MediatR;

namespace MangaBaseAPI.Application.Tittles.Commands.UpdateArtists
{
    public record UpdateTitleArtistsCommand(
        Guid Id,
        List<Guid> Artists) : IRequest<Result>;
}
