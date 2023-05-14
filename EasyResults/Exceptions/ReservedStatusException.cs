namespace EasyResults.Exceptions;

/// <summary>
/// Exception that means that the result status is reserved and cannot be used
/// </summary>
public class ReservedStatusException : Exception
{
    public ReservedStatusException() : base("The provided status is reserved and cannot be used") { }
}
