using MangaBaseAPI.Domain.Abstractions;
using MediatR;

namespace MangaBaseAPI.Application.Tittles.Commands.UpdateAuthors
{
    public record UpdateTitleAuthorsCommand(
        Guid Id,
        List<Guid> Authors) : IRequest<Result>;
}
