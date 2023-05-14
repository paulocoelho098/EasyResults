using EasyResults.Entities;
using EasyResults.Enums;
using EasyResults.Handlers;

namespace EasyResults.Tests;

public class UndefinedHandlersWithDefinedUnhittedHandlerTests
{
    [Fact]
    public void HandleResult_ReturnsResultHandlerResult_WhenStatusIsSuccessAndSuccessHandlerIsNotDefinedAndUnhittedHandlerIsDefined()
    {
        // Arrange
        Result result = ResultsMock.Success;
        string resultStr = "";

        ResultHandler resultHandler = new ResultHandler()
            .OnClientError(() => resultStr = "client")
            .OnServerError(() => resultStr = "server")
            .OnCustomStatus(() => resultStr = "custom")
            .OnUnhittedHandler(() => resultStr = "unhitted");

        // Act
        resultHandler.HandleResult(result);

        // Assert
        Assert.Equal("unhitted", resultStr);
    }

    [Theory]
    [InlineData(Status.BadRequest)]
    [InlineData(Status.Unauthorized)]
    [InlineData(Status.Forbidden)]
    [InlineData(Status.NotFound)]
    [InlineData(Status.Conflict)]
    public void HandleResult_ReturnsResultHandlerResult_WhenStatusIsClientErrorError_AndClientErrorHandlerIsNotDefinedAndUnhittedIsDefined(Status status)
    {
        // Arrange
        Result<string> result = new(status);
        string resultStr = "";

        ResultHandler resultHandler = new ResultHandler()
            .OnSuccess(() => resultStr = "success")
            .OnServerError(() => resultStr = "server")
            .OnCustomStatus(() => resultStr = "custom")
            .OnUnhittedHandler(() => resultStr = "unhitted");

        // Act
        resultHandler.HandleResult(result);

        // Assert
        Assert.Equal("unhitted", resultStr);
    }

    [Fact]
    public void HandleResult_ReturnsResultHandlerResult_WhenStatusIsInternalServerErrorAndInternalServerErrorHandlerIsNotDefinedAndUnhittedIsDefined()
    {
        // Arrange
        Result result = ResultsMock.InternalServerError;
        string resultStr = "";

        ResultHandler resultHandler = new ResultHandler()
            .OnSuccess(() => resultStr = "success")
            .OnClientError(() => resultStr = "client")
            .OnCustomStatus(() => resultStr = "custom")
            .OnUnhittedHandler(() => resultStr = "unhitted");

        // Act
        resultHandler.HandleResult(result);

        // Assert
        Assert.Equal("unhitted", resultStr);
    }

    [Fact]
    public void HandleResult_ReturnsResultHandlerResult_WhenStatusIsCustomAndCustomIsNotDefinedAndUnhittedIsDefined()
    {
        // Arrange
        Result<string> result = new Result<string>((Status)1000);
        string resultStr = "";

        ResultHandler resultHandler = new ResultHandler()
            .OnSuccess(() => resultStr = "success")
            .OnClientError(() => resultStr = "client")
            .OnServerError(() => resultStr = "server")
            .OnUnhittedHandler(() => resultStr = "unhitted");

        // Act
        resultHandler.HandleResult(result);

        // Assert
        Assert.Equal("unhitted", resultStr);
    }

}
