using EasyResults.Enums;

namespace EasyResults.Entities
{
    /// <summary>
    /// Represents a result of an operation
    /// </summary>
    /// <typeparam name="T">The type of the data that the result contains</typeparam>
    public class Result<T>
    {
        public Status Status { get; set; }
        public T? Data { get; set; } = default;
        public string? Message { get; set; }

        public Result() { }

        public Result(Status status, string message = "", T data = default)
        {
            Status = status;
            Message = message;
            Data = data;
        }
    }
}
