namespace EasyResults.Exceptions
{
    /// <summary>
    /// Exception that means that the Action was not defined before invoking Execute
    /// </summary>
    public class ActionNotDefined : Exception
    {
        public ActionNotDefined() : base("Action not defined before invoking Execute") { }
    }
}
