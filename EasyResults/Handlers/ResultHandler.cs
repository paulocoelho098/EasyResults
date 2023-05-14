using EasyResults.Enums;
using EasyResults.Exceptions;
using EasyResults.Interfaces;
using System.Diagnostics;

namespace EasyResults.Handlers;

/// <summary>
/// Class used to handle a return of type IResult
/// </summary>
public class ResultHandler
{
    /// <summary>
    /// Method that contains the logic that will return a IResult
    /// </summary>
    private Func<IResult>? _action { get; set; }

    /// <summary>
    /// Method that handles the result if it returns a Status of success
    /// </summary>
    private Action? _successResultHandler;

    /// <summary>
    /// Method that handles the result if it returns a Status of client error
    /// </summary>
    private Action? _clientErrorResultHandler;

    /// <summary>
    /// Method that handles the result if it returns a Status of server error
    /// </summary>
    private Action? _serverErrorResultHandler;

    /// <summary>
    /// Method that handles the result if it returns a custom Status
    /// </summary>
    private Action? _customResultHandler;

    /// <summary>
    /// Dictionary that maps Status codes to methods. If _action returns a Status code that exists
    /// in the dictionary then it will execute the corresponding method to handle the result
    /// </summary>
    private Dictionary<Status, Action> _statusResultHandlerMap;

    /// <summary>
    /// Method that handles the result of _action if there is none of the previous methods were hit
    /// </summary>
    private Action? _unhittedHandler;

    public ResultHandler()
    {
        _statusResultHandlerMap = new Dictionary<Status, Action>();
    }

    /// <summary>
    /// Executes the method defined in _action and Handle its result
    /// </summary>
    /// <exception cref="ActionNotDefined">If _action is not defined then there isn't any method to execute</exception>
    public void Execute()
    {
        if (_action is null)
        {
            throw new ActionNotDefined();
        }

        // Execute the main action that will retrieve an IResult to handle
        IResult result = _action.Invoke();

        HandleResult(result);
    }

    /// <summary>
    /// Invokes the result handler method according to the received result
    /// </summary>
    /// <exception cref="UnreachableException">When the Status Code returned by _action if outside bounds</exception>
    /// <exception cref="NotHandledException">When the result is not handled by any Handler</exception>
    public void HandleResult(IResult result)
    {
        #region Handle by Status Code

        _statusResultHandlerMap.TryGetValue(result.Status, out Action? statusResultHandler);

        if (statusResultHandler is not null)
        {
            statusResultHandler.Invoke();
            return;
        }

        #endregion

        #region Handle ranges of status

        switch (result.Status)
        {
            case Status.Success:
                if (_successResultHandler is not null)
                {
                    _successResultHandler.Invoke();
                    return;
                }
                break;

            case Status.BadRequest:
            case Status.Unauthorized:
            case Status.Forbidden:
            case Status.NotFound:
            case Status.Conflict:
                if (_clientErrorResultHandler is not null)
                {
                    _clientErrorResultHandler.Invoke();
                    return;
                }
                break;

            case Status.InternalServerError:
                if (_serverErrorResultHandler is not null)
                {
                    _serverErrorResultHandler.Invoke();
                    return;
                }
                break;

            default:
                if ((int)result.Status >= 8 && (int)result.Status <= 999)
                {
                    throw new ReservedStatusException();
                }
                if (_customResultHandler is not null)
                {
                    _customResultHandler.Invoke();
                    return;
                }
                break;
        }

        #endregion

        #region Handle unhitted handlers

        if (_unhittedHandler is not null)
        {
            _unhittedHandler.Invoke();
            return;
        }

        #endregion

        throw new NotHandledException();
    }

    /// <summary>
    /// Sets the action method
    /// </summary>
    /// <param name="action">Method that executes the action</param>
    /// <returns>Instance of the current ResultHandler</returns>
    public ResultHandler Action(Func<IResult> action)
    {
        _action = action;
        return this;
    }

    /// <summary>
    /// Sets the method that will handle success status
    /// </summary>
    /// <param name="action">Handler method</param>
    /// <returns>Instance of the current ResultHandler</returns>
    public ResultHandler OnSuccess(Action action)
    {
        _successResultHandler = action;
        return this;
    }

    /// <summary>
    /// Sets the method that will handle results with status related to client error
    /// </summary>
    /// <param name="action">Handler method</param>
    /// <returns>Instance of the current ResultHandler</returns>
    public ResultHandler OnClientError(Action action)
    {
        _clientErrorResultHandler = action;
        return this;
    }

    /// <summary>
    /// Sets the method that will handle results with status related with server error
    /// </summary>
    /// <param name="action">Handler method</param>
    /// <returns>Instance of the current ResultHandler</returns>
    public ResultHandler OnServerError(Action action)
    {
        _serverErrorResultHandler = action;
        return this;
    }

    /// <summary>
    /// Sets the method that will handle results with custom status
    /// </summary>
    /// <param name="action">Handler method</param>
    /// <returns>Instance of the current ResultHandler</returns>
    public ResultHandler OnCustomStatus(Action action)
    {
        _customResultHandler = action;
        return this;
    }

    /// <summary>
    /// Sets the method that sets a handler for a specific Status
    /// </summary>
    /// <param name="action">Handler method</param>
    /// <returns>Instance of the current ResultHandler</returns>
    public ResultHandler OnStatus(Status status, Action action)
    {
        _statusResultHandlerMap[status] = action;
        return this;
    }

    /// <summary>
    /// Sets the method that will handle the result if none of the handlers were hit
    /// </summary>
    /// <param name="action">Handler method</param>
    /// <returns>Instance of the current ResultHandler</returns>
    public ResultHandler OnUnhittedHandler(Action action)
    {
        _unhittedHandler = action;
        return this;
    }

}
