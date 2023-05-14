using EasyResults.Entities;
using EasyResults.Enums;
using EasyResults.Exceptions;
using EasyResults.Handlers;

namespace EasyResults.Tests;

public class MisconfiguredResultHandlerTests
{
    [Fact]
    public void Execute_ThrowsActionNotDefinedException_WhenActionIsNull()
    {
        // Arrange
        ResultHandler resultHandler = new();

        // Act & Assert
        Assert.Throws<ActionNotDefined>(() => resultHandler.Execute());
    }

    [Fact]
    public void HandleResult_ThrowsNotHandledException_WhenStatusIsSuccessAndSuccessHandlerIsNull()
    {
        // Arrange
        Result result = ResultsMock.Success;
        string resultStr = "";

        ResultHandler resultHandler = new ResultHandler()
            .OnClientError(() => resultStr = "true")
            .OnServerError(() => resultStr = "false")
            .OnCustomStatus(() => resultStr = "custom");

        // Act & Assert
        Assert.Throws<NotHandledException>(() => resultHandler.HandleResult(result));
    }

    [Theory]
    [InlineData(Status.BadRequest)]
    [InlineData(Status.Unauthorized)]
    [InlineData(Status.NotFound)]
    [InlineData(Status.Conflict)]
    [InlineData(Status.Forbidden)]
    public void HandleResult_ThrowsNotHandledException_WhenStatusIsClientErrorAndClientErrorHandlerIsNull(Status status)
    {
        // Arrange
        Result<string> result = new(status);
        string resultStr = "";

        ResultHandler resultHandler = new ResultHandler()
            .OnSuccess(() => resultStr = "true")
            .OnServerError(() => resultStr = "false")
            .OnCustomStatus(() => resultStr = "custom");

        // Act & Assert
        Assert.Throws<NotHandledException>(() => resultHandler.HandleResult(result));
    }

    [Fact]
    public void HandleResult_ThrowsNotHandledException_WhenStatusIsInternalServerErrorAndServerErrorHandlerIsNull()
    {
        // Arrange
        Result result = ResultsMock.InternalServerError;

        string resultStr = "";
        ResultHandler resultHandler = new ResultHandler()
            .OnSuccess(() => resultStr = "true")
            .OnClientError(() => resultStr = "false")
            .OnCustomStatus(() => resultStr = "custom");

        // Act & Assert
        Assert.Throws<NotHandledException>(() => resultHandler.HandleResult(result));
    }

    [Fact]
    public void HandleResult_ThrowsNotHandledException_WhenStatusIsCustomAndCustomHandlerIsNull()
    {
        // Arrange
        Result<string> result = new Result<string>((Status)1000);
        string resultStr = "";

        ResultHandler resultHandler = new ResultHandler()
            .OnSuccess(() => resultStr = "true")
            .OnClientError(() => resultStr = "false")
            .OnServerError(() => resultStr = "false");

        // Act & Assert
        Assert.Throws<NotHandledException>(() => resultHandler.HandleResult(result));
    }

    [Theory]
    [InlineData(8)]
    [InlineData(9)]
    [InlineData(300)]
    [InlineData(600)]
    [InlineData(998)]
    [InlineData(999)]
    public void HandleResult_ThrowsReservedStatusException_WhenStatusIsCustomAndInReservedRange(int status)
    {
        // Arrange
        Result<string> result = new Result<string>((Status)status);
        string resultStr = "";

        ResultHandler resultHandler = new ResultHandler()
            .OnSuccess(() => resultStr = "true")
            .OnClientError(() => resultStr = "false")
            .OnServerError(() => resultStr = "false")
            .OnCustomStatus(() => resultStr = "custom");

        // Act & Assert
        Assert.Throws<ReservedStatusException>(() => resultHandler.HandleResult(result));
    }

}
