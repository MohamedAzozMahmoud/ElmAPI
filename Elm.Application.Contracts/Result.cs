namespace Elm.Application.Contracts
{

    public class Result : IResult
    {
        public bool IsSuccess { get; protected init; }
        public int StatusCode { get; protected init; }
        public List<ValidationError> Errors { get; protected init; } = [];
        public string? Message { get; protected init; }

        protected Result() { }

        public static Result Success() => new()
        {
            IsSuccess = true,
            StatusCode = 200
        };

        public static Result Failure(string message, int statusCode = 400) => new()
        {
            IsSuccess = false,
            StatusCode = statusCode,
            Message = message,
            Errors = [ValidationError.Create("General", message)]
        };

        public static Result Failure(List<ValidationError> errors, int statusCode = 400) => new()
        {
            IsSuccess = false,
            StatusCode = statusCode,
            Errors = errors,
            Message = errors.FirstOrDefault()?.ErrorMessage
        };
    }

    public class Result<T> : Result, IResult<T>
    {
        public T? Data { get; private init; }

        private Result() { }

        public static Result<T> Success(T data) => new()
        {
            IsSuccess = true,
            StatusCode = 200,
            Data = data
        };

        public static Result<T> Success(T data, string message) => new()
        {
            IsSuccess = true,
            StatusCode = 200,
            Data = data,
            Message = message
        };

        public static new Result<T> Failure(string message, int statusCode = 400) => new()
        {
            IsSuccess = false,
            StatusCode = statusCode,
            Message = message,
            Errors = [ValidationError.Create("General", message, "GENERAL_ERROR")]
        };

        public static new Result<T> Failure(List<ValidationError> errors, int statusCode = 400) => new()
        {
            IsSuccess = false,
            StatusCode = statusCode,
            Errors = errors,
            Message = string.Join(" | ", errors.Select(e => e.ErrorMessage))
        };

        public static Result<T> NotFound(string message = "Resource not found") => new()
        {
            IsSuccess = false,
            StatusCode = 404,
            Message = message,
            Errors = [ValidationError.Create("General", message, "NOT_FOUND")]
        };

        public static Result<T> Unauthorized(string message = "Unauthorized access") => new()
        {
            IsSuccess = false,
            StatusCode = 401,
            Message = message,
            Errors = [ValidationError.Create("General", message, "UNAUTHORIZED")]
        };

        public static Result<T> Forbidden(string message = "Access denied") => new()
        {
            IsSuccess = false,
            StatusCode = 403,
            Message = message,
            Errors = [ValidationError.Create("General", message, "FORBIDDEN")]
        };

        // ✅ Implicit conversion for cleaner code
        public static implicit operator Result<T>(T data) => Success(data);
    }

    //public class Result
    //{
    //    public bool IsSuccess { get; set; }
    //    public int ErrorCode { get; set; }
    //    public string Message { get; set; } = string.Empty;
    //    public Result(bool isSuccess, string message)
    //    {
    //        IsSuccess = isSuccess;
    //        Message = message;
    //    }
    //    public Result(bool isSuccess, int errorCode, string message) : this(isSuccess, message)
    //    {
    //        ErrorCode = errorCode;
    //    }
    //    public static Result Success() => new Result(true, 200, string.Empty);
    //    public static Result Failure(string error, int errorCode = 400)
    //                => new Result(false, errorCode, error);
    //}
    //public class Result<T> : Result
    //{
    //    public T? Data { get; set; }
    //    public Result(bool isSuccess, T? data, string message, int errorCode) : base(isSuccess, errorCode, message)
    //    {
    //        Data = data;
    //    }
    //    public static Result<T> Success(T data) => new Result<T>(true, data, string.Empty, 200);

    //    public static Result<T> Failure(string message, int errorCode = 400) => new Result<T>(false, default, message, errorCode);

    //}
}
