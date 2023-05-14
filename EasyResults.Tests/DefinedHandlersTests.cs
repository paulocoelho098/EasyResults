using EasyResults.Entities;
using EasyResults.Enums;
using EasyResults.Handlers;

namespace EasyResults.Tests;

public class DefinedHandlersTests
{
    [Fact]
    public void Execute_ReturnsResultHandlerResult_WhenActionIsDefined()
    {
        // Arrange
        ResultHandler<string> resultHandler = new ResultHandler<string>()
            .Action(() =>
            {
                return ResultsMock.Success;
            })
            .OnSuccess(r => "true");

        // Act
        string resultHandlerResult = resultHandler.Execute();

        // Assert
        Assert.Equal("true", resultHandlerResult);
    }

    [Theory]
    [InlineData(Status.Success, "success")]
    [InlineData(Status.BadRequest, "client")]
    [InlineData(Status.Unauthorized, "client")]
    [InlineData(Status.Forbidden, "client")]
    [InlineData(Status.NotFound, "client")]
    [InlineData(Status.Conflict, "client")]
    [InlineData(Status.InternalServerError, "server")]
    [InlineData((Status)1000, "custom")]
    public void HandleResult_ReturnsResultHandlerResult_WhenHandleResultIsDefined(Status status, string expected)
    {
        // Arrange
        Result<string> result = new(status);

        var resultHandler = new ResultHandler<string>()
        .OnSuccess(r => "success")
        .OnClientError(r => "client")
        .OnServerError(r => "server")
        .OnCustomStatus(r => "custom");

        // Act
        string resultHandlerResult = resultHandler.HandleResult(result);

        // Assert
        Assert.Equal(expected, resultHandlerResult);
    }

    [Theory]
    [InlineData(Status.Success, "success_1")]
    [InlineData(Status.BadRequest, "client_1")]
    [InlineData(Status.Unauthorized, "client_1")]
    [InlineData(Status.Forbidden, "client_1")]
    [InlineData(Status.NotFound, "client_1")]
    [InlineData(Status.Conflict, "client_1")]
    [InlineData(Status.InternalServerError, "server_1")]
    [InlineData((Status)1000, "custom_1")]
    public void HandleResult_ReturnsResultHandlerResult_WhenResultHasObject(Status status, string expected)
    {
        // Arrange
        TestId testId = new(1);
        Result<TestId> result = new(status, testId);

        var resultHandler = new ResultHandler<string>()
        .OnSuccess(r => $"success_{((Result<TestId>)r).Data!.Id}")
        .OnClientError(r => $"client_{((Result<TestId>)r).Data!.Id}")
        .OnServerError(r => $"server_{((Result<TestId>)r).Data!.Id}")
        .OnCustomStatus(r => $"custom_{((Result<TestId>)r).Data!.Id}");

        // Act
        string resultHandlerResult = resultHandler.HandleResult(result);

        // Assert
        Assert.Equal(expected, resultHandlerResult);
    }

}
