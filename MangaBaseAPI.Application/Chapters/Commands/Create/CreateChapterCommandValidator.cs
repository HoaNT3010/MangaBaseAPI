using FluentValidation;
using MangaBaseAPI.Application.Common.Utilities.Storage;

namespace MangaBaseAPI.Application.Chapters.Commands.Create
{
    public class CreateChapterCommandValidator : AbstractValidator<CreateChapterCommand>
    {
        public CreateChapterCommandValidator()
        {
            ClassLevelCascadeMode = CascadeMode.Stop;

            RuleFor(x => x.Name)
                .NotNull().WithMessage("Chapter's name must be included")
                .NotEmpty().WithMessage("Chapter's name cannot be empty")
                .MaximumLength(250).WithMessage("Chapter's name cannot exceed 250 characters");

            RuleFor(x => x.Volume)
                .GreaterThanOrEqualTo(0).WithMessage("Chapter's volume cannot be less than 0")
                .LessThanOrEqualTo(99).WithMessage("Chapter's volume cannot be greater than 99");

            RuleFor(x => x.Index)
                .GreaterThan(0).WithMessage("Chapter's index must be greater than 0")
                .LessThanOrEqualTo(9999).WithMessage("Chapter's index cannot be greater than 9999")
                .Must(x => IsWholeNumber(x) || HasOneDecimalPlace(x));

            RuleFor(x => x.ChapterImages)
                .Must(x => x.Count() > 0).WithMessage("Chapter's must contain at least 1 image")
                .Must(x => x.Count() <= 30).WithMessage("Chapter cannot have more than 30 images")
                .Must(x => x == null || x.Select(x => Path.GetFileNameWithoutExtension(x.FileName)).Distinct().ToList().Count == x.Count).WithMessage("Image names cannot be duplicated");

            RuleForEach(x => x.ChapterImages)
                .Must(x => x == null || ImageValidator.IsImageExtensionValid(x.FileName)).WithMessage("Invalid image extension. Only '.png', '.jpg', '.webp' image extensions are supported")
                .Must(x => x == null || ImageValidator.IsFileSizeValid(x)).WithMessage("Chapter image size cannot exceed 10 MB")
                .Must(x => x == null || ImageValidator.IsFileMimeTypeValid(x)).WithMessage("Invalid image content type. Only 'image/png', 'image/jpeg', 'image/webp' content types are allowed")
                .Must(x => x == null || (int.TryParse(Path.GetFileNameWithoutExtension(x.FileName), out int result) && result > 0)).WithMessage("Image name must be an integer number greater than 0");
        }

        static bool IsWholeNumber(float number)
        {
            return number == Math.Floor(number);
        }

        static bool HasOneDecimalPlace(float number)
        {
            float decimalPart = number - (float)Math.Floor(number);
            return Math.Abs(decimalPart * 10 - Math.Floor(decimalPart * 10)) < 1e-6;
        }
    }
}
