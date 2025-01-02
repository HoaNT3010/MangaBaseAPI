using MangaBaseAPI.Domain.Abstractions;
using MediatR;

namespace MangaBaseAPI.Application.Titles.Commands.UpdateAuthors
{
    public record UpdateTitleAuthorsCommand(
        Guid Id,
        List<Guid> Authors) : IRequest<Result>;
}
