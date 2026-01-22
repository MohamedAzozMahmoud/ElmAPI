namespace Elm.Application.Contracts
{
    public interface IResult
    {
        bool IsSuccess { get; }
        int StatusCode { get; }
        List<ValidationError> Errors { get; }
        string? Message { get; }
    }
    public interface IResult<out T> : IResult
    {
        T? Data { get; }
    }
}
