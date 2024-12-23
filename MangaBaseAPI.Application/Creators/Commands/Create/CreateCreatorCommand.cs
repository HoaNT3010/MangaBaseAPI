using MangaBaseAPI.Domain.Abstractions;
using MediatR;

namespace MangaBaseAPI.Application.Creators.Commands.Create
{
    public record CreateCreatorCommand(
        string Name,
        string? Biography) : IRequest<Result>;
}
