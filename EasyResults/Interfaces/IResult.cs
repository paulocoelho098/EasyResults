using EasyResults.Enums;

namespace EasyResults.Interfaces;

/// <summary>
/// Represents a result of an operation
/// </summary>
public interface IResult
{
    /// <summary>
    /// Status of the result
    /// </summary>
    Status Status { get; set; }

    /// <summary>
    /// Message to return with the result
    /// </summary>
    string? Message { get; set; }
}
