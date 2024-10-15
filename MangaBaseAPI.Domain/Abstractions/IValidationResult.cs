namespace MangaBaseAPI.Domain.Abstractions
{
    public interface IValidationResult
    {
        public static readonly Error ValidationError = Error.Validation("ValidationError", "Validation problem(s) occurred");

        Error[] Errors { get; }
    }
}
