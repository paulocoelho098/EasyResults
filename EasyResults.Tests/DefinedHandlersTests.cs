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
        string result = "";
        ResultHandler resultHandler = new ResultHandler()
            .Action(() =>
            {
                return ResultsMock.Success;
            })
            .OnSuccess(() => result = "true");

        // Act
        resultHandler.Execute();

        // Assert
        Assert.Equal("true", result);
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
        string resultStr = "";

        ResultHandler resultHandler = new ResultHandler()
            .OnSuccess(() => resultStr = "success")
            .OnClientError(() => resultStr = "client")
            .OnServerError(() => resultStr = "server")
            .OnCustomStatus(() => resultStr = "custom");

        // Act
        resultHandler.HandleResult(result);

        // Assert
        Assert.Equal(expected, resultStr);
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
        string resultStr = "";

        ResultHandler resultHandler = new ResultHandler()
        .OnSuccess(() => resultStr = $"success_{result.Data!.Id}")
        .OnClientError(() => resultStr = $"client_{result.Data!.Id}")
        .OnServerError(() => resultStr = $"server_{result.Data!.Id}")
        .OnCustomStatus(() => resultStr = $"custom_{result.Data!.Id}");

        // Act
        resultHandler.HandleResult(result);

        // Assert
        Assert.Equal(expected, resultStr);
    }

}
