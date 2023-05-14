using EasyResults.Enums;
using EasyResults.Interfaces;

namespace EasyResults.Entities;

public class Result : IResult
{
    public Status Status { get; set; }
    public string? Message { get; set; }

    public Result() { }

    public Result(Status status, string message)
    {
        Status = status;
        Message = message;
    }

    public Result(Status status)
    {
        Status = status;
    }
}

public class Result<T> : Result
{
    public T? Data { get; set; } = default;

    public Result() { }

    public Result(Status status, string message, T data) : base(status, message)
    {
        Data = data;
    }

    public Result(Status status, string message) : base(status, message)
    {
    }

    public Result(Status status, T data) : base(status)
    {
        Data = data;
    }

    public Result(Status status) : base(status)
    {
    }
}
