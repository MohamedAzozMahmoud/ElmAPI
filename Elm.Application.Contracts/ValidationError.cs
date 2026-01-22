namespace Elm.Application.Contracts
{
    public sealed record ValidationError
    {
        public string PropertyName { get; init; } = string.Empty;
        public string ErrorMessage { get; init; } = string.Empty;
        public string ErrorCode { get; init; } = string.Empty;
        public object? AttemptedValue { get; init; }
        public Severity Severity { get; init; } = Severity.Error;

        public static ValidationError Create(
            string propertyName,
            string errorMessage,
            string? errorCode = null,
            object? attemptedValue = null) => new()
            {
                PropertyName = propertyName,
                ErrorMessage = errorMessage,
                ErrorCode = errorCode ?? "VALIDATION_ERROR",
                AttemptedValue = attemptedValue
            };
    }

}
