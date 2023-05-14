using EasyResults.Entities;
using EasyResults.Enums;
using EasyResults.Handlers;

namespace EasyResults.Tests;

public class DefinedStatusHandlerTests
{
    [Fact]
    public void HandleResult_ReturnsResultHandlerResult_WhenStatusIsSuccessAndSuccessStatusHandlerIsDefined()
    {
        // Arrange
        Result result = ResultsMock.Success;
        string resultStr = "";

        ResultHandler resultHandler = new ResultHandler()
            .OnStatus(Status.Success, () => resultStr = "success");

        // Act
        resultHandler.HandleResult(result);

        // Assert
        Assert.Equal("success", resultStr);
    }

    [Fact]
    public void HandleResult_ReturnsResultHandlerResult_WhenStatusIsBadRequestAndBadRequestStatusHandlerIsDefined()
    {
        // Arrange
        Result result = ResultsMock.BadRequest;
        string resultStr = "";

        ResultHandler resultHandler = new ResultHandler()
            .OnSuccess(() => resultStr = "success")
            .OnClientError(() => resultStr = "client")
            .OnServerError(() => resultStr = "server")
            .OnCustomStatus(() => resultStr = "custom")
            .OnStatus(Status.BadRequest, () => resultStr = "badrequest");

        // Act
        resultHandler.HandleResult(result);

        // Assert
        Assert.Equal("badrequest", resultStr);
    }

    [Fact]
    public void HandleResult_ReturnsResultHandlerResult_WhenStatusIsUnauthorizedAndUnauthorizedStatusHandlerIsDefined()
    {
        // Arrange
        Result result = ResultsMock.Unauthorized;
        string resultStr = "";

        ResultHandler resultHandler = new ResultHandler()
            .OnSuccess(() => resultStr = "success")
            .OnClientError(() => resultStr = "client")
            .OnServerError(() => resultStr = "server")
            .OnCustomStatus(() => resultStr = "custom")
            .OnStatus(Status.Unauthorized, () => resultStr = "unauthorized");

        // Act
        resultHandler.HandleResult(result);

        // Assert
        Assert.Equal("unauthorized", resultStr);
    }

    [Fact]
    public void HandleResult_ReturnsResultHandlerResult_WhenStatusIsForbiddenAndForbiddenStatusHandlerIsDefined()
    {
        // Arrange
        Result result = ResultsMock.Forbidden;
        string resultStr = "";

        ResultHandler resultHandler = new ResultHandler()
            .OnSuccess(() => resultStr = "success")
            .OnClientError(() => resultStr = "client")
            .OnServerError(() => resultStr = "server")
            .OnCustomStatus(() => resultStr = "custom")
            .OnStatus(Status.Forbidden, () => resultStr = "forbidden");

        // Act
        resultHandler.HandleResult(result);

        // Assert
        Assert.Equal("forbidden", resultStr);
    }

    [Fact]
    public void HandleResult_ReturnsResultHandlerResult_WhenStatusIsNotFoundAndNotFoundStatusHandlerIsDefined()
    {
        // Arrange
        Result result = ResultsMock.NotFound;
        string resultStr = "";

        ResultHandler resultHandler = new ResultHandler()
            .OnSuccess(() => resultStr = "success")
            .OnClientError(() => resultStr = "client")
            .OnServerError(() => resultStr = "server")
            .OnCustomStatus(() => resultStr = "custom")
            .OnStatus(Status.NotFound, () => resultStr = "notfound");

        // Act
        resultHandler.HandleResult(result);

        // Assert
        Assert.Equal("notfound", resultStr);
    }

    [Fact]
    public void HandleResult_ReturnsResultHandlerResult_WhenStatusIsConflictAndConflictStatusHandlerIsDefined()
    {
        // Arrange
        Result result = ResultsMock.Conflict;
        string resultStr = "";

        ResultHandler resultHandler = new ResultHandler()
            .OnSuccess(() => resultStr = "success")
            .OnClientError(() => resultStr = "client")
            .OnServerError(() => resultStr = "server")
            .OnCustomStatus(() => resultStr = "custom")
            .OnStatus(Status.Conflict, () => resultStr = "conflict");

        // Act
        resultHandler.HandleResult(result);

        // Assert
        Assert.Equal("conflict", resultStr);
    }

    [Fact]
    public void HandleResult_ReturnsResultHandlerResult_WhenStatusIsInternalServerErrorAndInternalServerErrorStatusHandlerIsDefined()
    {
        // Arrange
        Result result = ResultsMock.InternalServerError;
        string resultStr = "";

        ResultHandler resultHandler = new ResultHandler()
            .OnSuccess(() => resultStr = "success")
            .OnClientError(() => resultStr = "client")
            .OnServerError(() => resultStr = "server")
            .OnCustomStatus(() => resultStr = "custom")
            .OnStatus(Status.InternalServerError, () => resultStr = "internalservererror");

        // Act
        resultHandler.HandleResult(result);

        // Assert
        Assert.Equal("internalservererror", resultStr);
    }

    [Fact]
    public void HandleResult_ReturnsResultHandlerResult_WhenStatusIsCustomAndCustomStatusHandlerIsDefined()
    {
        // Arrange
        Result<string> result = new Result<string>((Status)1000);
        string resultStr = "";

        ResultHandler resultHandler = new ResultHandler()
            .OnSuccess(() => resultStr = "success")
            .OnClientError(() => resultStr = "client")
            .OnServerError(() => resultStr = "server")
            .OnCustomStatus(() => resultStr = "custom")
            .OnStatus((Status)1000, () => resultStr = "customStatus");

        // Act
        resultHandler.HandleResult(result);

        // Assert
        Assert.Equal("customStatus", resultStr);
    }

}
