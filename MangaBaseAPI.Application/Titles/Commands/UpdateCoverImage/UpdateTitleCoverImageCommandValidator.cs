using FluentValidation;
using MangaBaseAPI.Application.Common.Utilities.Storage;

namespace MangaBaseAPI.Application.Titles.Commands.UpdateCoverImage
{
    public class UpdateTitleCoverImageCommandValidator : AbstractValidator<UpdateTitleCoverImageCommand>
    {
        public UpdateTitleCoverImageCommandValidator()
        {
            RuleFor(x => x.File)
                .NotNull().WithMessage("Cover image must be included")
                .Must(x => ImageValidator.IsImageExtensionValid(x.FileName)).WithMessage("Invalid file extension. Only '.png', '.jpg', '.webp' file extensions are supported")
                .Must(ImageValidator.IsFileSizeValid).WithMessage("Cover image size cannot exceed 10 MB")
                .Must(ImageValidator.IsFileMimeTypeValid).WithMessage("Invalid file content type. Only 'image/png', 'image/jpeg', 'image/webp' content types are allowed");
            // Consider adding additional constraints such as dimensions, aspect ratio,...
        }
    }
}
