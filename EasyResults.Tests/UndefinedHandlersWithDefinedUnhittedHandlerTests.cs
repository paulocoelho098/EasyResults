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
        var resultHandler = new ResultHandler<string>()
            .OnClientError(r => "client")
            .OnServerError(r => "server")
            .OnCustomStatus(r => "custom")
            .OnUnhittedHandler(r => "unhitted");

        // Act
        string resultHandlerResult = resultHandler.HandleResult(result);

        // Assert
        Assert.Equal("unhitted", resultHandlerResult);
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
        var resultHandler = new ResultHandler<string>()
            .OnSuccess(r => "success")
            .OnServerError(r => "server")
            .OnCustomStatus(r => "custom")
            .OnUnhittedHandler(r => "unhitted");

        // Act
        string resultHandlerResult = resultHandler.HandleResult(result);

        // Assert
        Assert.Equal("unhitted", resultHandlerResult);
    }

    [Fact]
    public void HandleResult_ReturnsResultHandlerResult_WhenStatusIsInternalServerErrorAndInternalServerErrorHandlerIsNotDefinedAndUnhittedIsDefined()
    {
        // Arrange
        Result result = ResultsMock.InternalServerError;
        var resultHandler = new ResultHandler<string>()
            .OnSuccess(r => "success")
            .OnClientError(r => "client")
            .OnCustomStatus(r => "custom")
            .OnUnhittedHandler(r => "unhitted");

        // Act
        string resultHandlerResult = resultHandler.HandleResult(result);

        // Assert
        Assert.Equal("unhitted", resultHandlerResult);
    }

    [Fact]
    public void HandleResult_ReturnsResultHandlerResult_WhenStatusIsCustomAndCustomIsNotDefinedAndUnhittedIsDefined()
    {
        // Arrange
        Result<string> result = new Result<string>((Status)1000);
        var resultHandler = new ResultHandler<string>()
            .OnSuccess(r => "success")
            .OnClientError(r => "client")
            .OnServerError(r => "server")
            .OnUnhittedHandler(r => "unhitted");

        // Act
        string resultHandlerResult = resultHandler.HandleResult(result);

        // Assert
        Assert.Equal("unhitted", resultHandlerResult);
    }

}
