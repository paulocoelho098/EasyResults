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
        var resultHandler = new ResultHandler<string>()
            .OnStatus(Status.Success, r => "success");

        // Act
        string resultHandlerResult = resultHandler.HandleResult(result);

        // Assert
        Assert.Equal("success", resultHandlerResult);
    }

    [Fact]
    public void HandleResult_ReturnsResultHandlerResult_WhenStatusIsBadRequestAndBadRequestStatusHandlerIsDefined()
    {
        // Arrange
        Result result = ResultsMock.BadRequest;
        var resultHandler = new ResultHandler<string>()
            .OnSuccess(r => "success")
            .OnClientError(r => "client")
            .OnServerError(r => "server")
            .OnCustomStatus(r => "custom")
            .OnStatus(Status.BadRequest, r => "badrequest");

        // Act
        string resultHandlerResult = resultHandler.HandleResult(result);

        // Assert
        Assert.Equal("badrequest", resultHandlerResult);
    }

    [Fact]
    public void HandleResult_ReturnsResultHandlerResult_WhenStatusIsUnauthorizedAndUnauthorizedStatusHandlerIsDefined()
    {
        // Arrange
        Result result = ResultsMock.Unauthorized;
        var resultHandler = new ResultHandler<string>()
            .OnSuccess(r => "success")
            .OnClientError(r => "client")
            .OnServerError(r => "server")
            .OnCustomStatus(r => "custom")
            .OnStatus(Status.Unauthorized, r => "unauthorized");

        // Act
        string resultHandlerResult = resultHandler.HandleResult(result);

        // Assert
        Assert.Equal("unauthorized", resultHandlerResult);
    }

    [Fact]
    public void HandleResult_ReturnsResultHandlerResult_WhenStatusIsForbiddenAndForbiddenStatusHandlerIsDefined()
    {
        // Arrange
        Result result = ResultsMock.Forbidden;
        var resultHandler = new ResultHandler<string>()
            .OnSuccess(r => "success")
            .OnClientError(r => "client")
            .OnServerError(r => "server")
            .OnCustomStatus(r => "custom")
            .OnStatus(Status.Forbidden, r => "forbidden");

        // Act
        string resultHandlerResult = resultHandler.HandleResult(result);

        // Assert
        Assert.Equal("forbidden", resultHandlerResult);
    }

    [Fact]
    public void HandleResult_ReturnsResultHandlerResult_WhenStatusIsNotFoundAndNotFoundStatusHandlerIsDefined()
    {
        // Arrange
        Result result = ResultsMock.NotFound;
        var resultHandler = new ResultHandler<string>()
            .OnSuccess(r => "success")
            .OnClientError(r => "client")
            .OnServerError(r => "server")
            .OnCustomStatus(r => "custom")
            .OnStatus(Status.NotFound, r => "notfound");

        // Act
        string resultHandlerResult = resultHandler.HandleResult(result);

        // Assert
        Assert.Equal("notfound", resultHandlerResult);
    }

    [Fact]
    public void HandleResult_ReturnsResultHandlerResult_WhenStatusIsConflictAndConflictStatusHandlerIsDefined()
    {
        // Arrange
        Result result = ResultsMock.Conflict;
        var resultHandler = new ResultHandler<string>()
            .OnSuccess(r => "success")
            .OnClientError(r => "client")
            .OnServerError(r => "server")
            .OnCustomStatus(r => "custom")
            .OnStatus(Status.Conflict, r => "conflict");

        // Act
        string resultHandlerResult = resultHandler.HandleResult(result);

        // Assert
        Assert.Equal("conflict", resultHandlerResult);
    }

    [Fact]
    public void HandleResult_ReturnsResultHandlerResult_WhenStatusIsInternalServerErrorAndInternalServerErrorStatusHandlerIsDefined()
    {
        // Arrange
        Result result = ResultsMock.InternalServerError;
        var resultHandler = new ResultHandler<string>()
            .OnSuccess(r => "success")
            .OnClientError(r => "client")
            .OnServerError(r => "server")
            .OnCustomStatus(r => "custom")
            .OnStatus(Status.InternalServerError, r => "internalservererror");

        // Act
        string resultHandlerResult = resultHandler.HandleResult(result);

        // Assert
        Assert.Equal("internalservererror", resultHandlerResult);
    }

    [Fact]
    public void HandleResult_ReturnsResultHandlerResult_WhenStatusIsCustomAndCustomStatusHandlerIsDefined()
    {
        // Arrange
        Result<string> result = new Result<string>((Status)1000);
        var resultHandler = new ResultHandler<string>()
            .OnSuccess(r => "success")
            .OnClientError(r => "client")
            .OnServerError(r => "server")
            .OnCustomStatus(r => "custom")
            .OnStatus((Status)1000, r => "customStatus");

        // Act
        string resultHandlerResult = resultHandler.HandleResult(result);

        // Assert
        Assert.Equal("customStatus", resultHandlerResult);
    }

}
