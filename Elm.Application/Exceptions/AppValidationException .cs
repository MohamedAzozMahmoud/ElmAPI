using Elm.Application.Contracts;

namespace Elm.Application.Exceptions
{

    public sealed class AppValidationException : Exception
    {
        public List<ValidationError> Errors { get; }
        public int StatusCode { get; }

        public AppValidationException(List<ValidationError> errors)
            : base("One or more validation errors occurred.")
        {
            Errors = errors;
            StatusCode = 400;
        }

        public AppValidationException(string propertyName, string errorMessage)
            : base(errorMessage)
        {
            Errors = [ValidationError.Create(propertyName, errorMessage)];
            StatusCode = 400;
        }
    }
}
