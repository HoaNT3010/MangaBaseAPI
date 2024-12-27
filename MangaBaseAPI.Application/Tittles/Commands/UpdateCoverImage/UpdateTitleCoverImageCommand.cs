using MangaBaseAPI.Domain.Abstractions;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace MangaBaseAPI.Application.Tittles.Commands.UpdateCoverImage
{
    public record UpdateTitleCoverImageCommand(
        Guid Id,
        IFormFile File) : IRequest<Result>;
}
