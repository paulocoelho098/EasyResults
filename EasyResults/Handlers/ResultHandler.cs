using EasyResults.Entities;
using EasyResults.Enums;
using EasyResults.Exceptions;
using System.Diagnostics;

namespace EasyResults.Handlers
{
    /// <summary>
    /// Class used to handle a return of type Result<T>
    /// </summary>
    /// <typeparam name="T">Type of the data returned by Result</typeparam>
    /// <typeparam name="T2">Type of the return of the ResultHandler</typeparam>
    public class ResultHandler<T, T2>
    {
        /// <summary>
        /// Method that contains the logic that will return a Result<T>
        /// </summary>
        private Func<Result<T>>? _action { get; set; }

        /// <summary>
        /// Method that handles the result if it returns a Status of success
        /// </summary>
        private Func<Result<T>, T2>? _successResultHandler;

        /// <summary>
        /// Method that handles the result if it returns a Status of client error
        /// </summary>
        private Func<Result<T>, T2>? _clientErrorResultHandler;

        /// <summary>
        /// Method that handles the result if it returns a Status of server error
        /// </summary>
        private Func<Result<T>, T2>? _serverErrorResultHandler;

        /// <summary>
        /// Method that handles the result if it returns a custom Status
        /// </summary>
        private Func<Result<T>, T2>? _customResultHandler;

        /// <summary>
        /// Dictionary that maps Status codes to methods. If _action returns a Status code that exists
        /// in the dictionary then it will execute the corresponding method to handle the result
        /// </summary>
        private Dictionary<Status, Func<Result<T>, T2>> _statusResultHandlerMap;

        /// <summary>
        /// Method that handles the result of _action if there is none of the previous methods were hit
        /// </summary>
        private Func<Result<T>, T2>? _unhittedHandler;

        public ResultHandler()
        {
            _statusResultHandlerMap = new Dictionary<Status, Func<Result<T>, T2>>();
        }

        /// <summary>
        /// Executes the method defined in _action and Handle its result
        /// </summary>
        /// <returns>ResultHandler result</returns>
        /// <exception cref="ActionNotDefined">If _action is not defined then there isn't any method to execute</exception>
        public T2 Execute()
        {
            if (_action is null)
            {
                throw new ActionNotDefined();
            }

            // Execute the main action that will retrieve a Result<T> to handle
            Result<T> result = _action.Invoke();

            return HandleResult(result);

        }

        /// <summary>
        /// Invokes the result handler method according to the received result
        /// </summary>
        /// <returns>ResultHandler result</returns>
        /// <exception cref="UnreachableException">When the Status Code returned by _action if outside bounds</exception>
        /// <exception cref="NotHandledException">When the result is not handled by any Handler</exception>
        public T2 HandleResult(Result<T> result)
        {
            #region Handle by Status Code

            _statusResultHandlerMap.TryGetValue(result.Status, out Func<Result<T>, T2>? statusResultHandler);

            if (statusResultHandler is not null)
            {
                return statusResultHandler.Invoke(result);
            }

            #endregion

            #region Handle ranges of status

            switch (result.Status)
            {
                case Status.Success:
                    if (_successResultHandler is not null)
                    {
                        return _successResultHandler.Invoke(result);
                    }
                    break;

                case Status.BadRequest:
                case Status.Unauthorized:
                case Status.Forbidden:
                case Status.NotFound:
                case Status.Conflict:
                    if (_clientErrorResultHandler is not null)
                    {
                        return _clientErrorResultHandler.Invoke(result);
                    }
                    break;

                case Status.InternalServerError:
                    if (_serverErrorResultHandler is not null)
                    {
                        return _serverErrorResultHandler.Invoke(result);
                    }
                    break;

                default:
                    if((int)result.Status >= 8 && (int)result.Status <= 999)
                    {
                        throw new ReservedStatusException();
                    }
                    if (_customResultHandler is not null)
                    {
                        return _customResultHandler.Invoke(result);
                    }
                    break;
            }

            #endregion

            #region Handle unhitted handlers

            if (_unhittedHandler is not null)
            {
                return _unhittedHandler.Invoke(result);
            }

            #endregion

            throw new NotHandledException();
        }

        /// <summary>
        /// Sets the action method
        /// </summary>
        /// <param name="action">Method that executes the action</param>
        /// <returns>Instance of the current ResultHandler</returns>
        public ResultHandler<T, T2> Action(Func<Result<T>> action)
        {
            _action = action;
            return this;
        }

        /// <summary>
        /// Sets the method that will handle success status
        /// </summary>
        /// <param name="action">Handler method</param>
        /// <returns>Instance of the current ResultHandler</returns>
        public ResultHandler<T, T2> OnSuccess(Func<Result<T>, T2> action)
        {
            _successResultHandler = action;
            return this;
        }

        /// <summary>
        /// Sets the method that will handle results with status related to client error
        /// </summary>
        /// <param name="action">Handler method</param>
        /// <returns>Instance of the current ResultHandler</returns>
        public ResultHandler<T, T2> OnClientError(Func<Result<T>, T2> action)
        {
            _clientErrorResultHandler = action;
            return this;
        }

        /// <summary>
        /// Sets the method that will handle results with status related with server error
        /// </summary>
        /// <param name="action">Handler method</param>
        /// <returns>Instance of the current ResultHandler</returns>
        public ResultHandler<T, T2> OnServerError(Func<Result<T>, T2> action)
        {
            _serverErrorResultHandler = action;
            return this;
        }

        /// <summary>
        /// Sets the method that will handle results with custom status
        /// </summary>
        /// <param name="action">Handler method</param>
        /// <returns>Instance of the current ResultHandler</returns>
        public ResultHandler<T, T2> OnCustomStatus(Func<Result<T>, T2> action)
        {
            _customResultHandler = action;
            return this;
        }

        /// <summary>
        /// Sets the method that sets a handler for a specific Status
        /// </summary>
        /// <param name="action">Handler method</param>
        /// <returns>Instance of the current ResultHandler</returns>
        public ResultHandler<T, T2> OnStatus(Status status, Func<Result<T>, T2> action)
        {
            _statusResultHandlerMap[status] = action;
            return this;
        }

        /// <summary>
        /// Sets the method that will handle the result if none of the handlers were hit
        /// </summary>
        /// <param name="action">Handler method</param>
        /// <returns>Instance of the current ResultHandler</returns>
        public ResultHandler<T, T2> OnUnhittedHandler(Func<Result<T>, T2> action)
        {
            _unhittedHandler = action;
            return this;
        }

    }
}
