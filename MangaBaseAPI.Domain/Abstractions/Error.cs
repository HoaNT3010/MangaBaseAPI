namespace MangaBaseAPI.Domain.Abstractions
{
    public record Error
    {
        public static readonly Error? Null = null;
        public static readonly Error None = new(string.Empty, string.Empty, ErrorType.None);
        public static readonly Error NullValue = new("Error.NullValue", "Null value was provided", ErrorType.Failure);
        public static readonly Error ConditionNotMet = new("Error.ConditionNotMet", "The specified condition was not met", ErrorType.Failure);

        private Error(string code, string description, ErrorType errorType)
        {
            Code = code;
            Description = description;
            Type = errorType;
        }

        public string Code { get; }

        public string Description { get; }

        public ErrorType Type { get; }

        public static Error NotFound(string code, string description) =>
            new(code, description, ErrorType.NotFound);

        public static Error Validation(string code, string description) =>
            new(code, description, ErrorType.Validation);

        public static Error Conflict(string code, string description) =>
            new(code, description, ErrorType.Conflict);

        public static Error Failure(string code, string description) =>
            new(code, description, ErrorType.Failure);

        public static Error Unauthorized(string code, string description) =>
            new(code, description, ErrorType.Unauthorized);
    }

    public enum ErrorType
    {
        None = 0,
        Failure = 1,
        Validation = 2,
        NotFound = 3,
        Conflict = 4,
        Unauthorized = 5,
    }
}
