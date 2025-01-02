using MangaBaseAPI.Domain.Abstractions;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace MangaBaseAPI.Application.Titles.Commands.UpdateCoverImage
{
    public record UpdateTitleCoverImageCommand(
        Guid Id,
        IFormFile File) : IRequest<Result>;
}
