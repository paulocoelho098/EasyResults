namespace EasyResults.Exceptions;

/// <summary>
/// Exception that means that the result was not handled by any Handler
/// </summary>
public class NotHandledException : Exception
{
    public NotHandledException() : base("The result must be handled before leaving the flow") { }
}

