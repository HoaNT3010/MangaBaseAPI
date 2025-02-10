using FluentValidation;
using MangaBaseAPI.Application.Common.Utilities.Storage;

namespace MangaBaseAPI.Application.Titles.Commands.UpdateCoverImage
{
    public class UpdateTitleCoverImageCommandValidator : AbstractValidator<UpdateTitleCoverImageCommand>
    {
        public UpdateTitleCoverImageCommandValidator()
        {
            ClassLevelCascadeMode = CascadeMode.Stop;

            RuleFor(x => x.File)
                .NotNull().WithMessage("Cover image must be included")
                .Must(x => x == null || ImageValidator.IsImageExtensionValid(x.FileName)).WithMessage("Invalid file extension. Only '.png', '.jpg', '.webp' file extensions are supported")
                .Must(x => x == null || ImageValidator.IsFileSizeValid(x)).WithMessage("Cover image size cannot exceed 10 MB")
                .Must(x => x == null || ImageValidator.IsFileMimeTypeValid(x)).WithMessage("Invalid file content type. Only 'image/png', 'image/jpeg', 'image/webp' content types are allowed");
            // Consider adding additional constraints such as dimensions, aspect ratio,...
        }
    }
}
