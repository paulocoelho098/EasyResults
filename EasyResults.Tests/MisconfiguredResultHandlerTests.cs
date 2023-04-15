using EasyResults.Entities;
using EasyResults.Enums;
using EasyResults.Exceptions;
using EasyResults.Handlers;
using System.Diagnostics;

namespace EasyResults.Tests
{
    public class MisconfiguredResultHandlerTests
    {
        [Fact]
        public void Execute_ThrowsActionNotDefinedException_WhenActionIsNull()
        {
            // Arrange
            ResultHandler<object, object> resultHandler = new();

            // Act & Assert
            Assert.Throws<ActionNotDefined>(() => resultHandler.Execute());
        }

        [Fact]
        public void HandleResult_ThrowsNotHandledException_WhenStatusIsSuccessAndSuccessHandlerIsNull()
        {
            // Arrange
            Result<string> result = ResultsMock.Success;
            ResultHandler<string, string> resultHandler = new ResultHandler<string, string>()
                .OnClientError(_ => "true")
                .OnServerError(_ => "false")
                .OnCustomStatus(_ => "custom");

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
            ResultHandler<string, string> resultHandler = new ResultHandler<string, string>()
                .OnSuccess(_ => "true")
                .OnServerError(_ => "false")
                .OnCustomStatus(_ => "custom");

            // Act & Assert
            Assert.Throws<NotHandledException>(() => resultHandler.HandleResult(result));
        }

        [Fact]
        public void HandleResult_ThrowsNotHandledException_WhenStatusIsInternalServerErrorAndServerErrorHandlerIsNull()
        {
            // Arrange
            Result<string> result = ResultsMock.InternalServerError;
            ResultHandler<string, string> resultHandler = new ResultHandler<string, string>()
                .OnSuccess(_ => "true")
                .OnClientError(_ => "false")
                .OnCustomStatus(_ => "custom");

            // Act & Assert
            Assert.Throws<NotHandledException>(() => resultHandler.HandleResult(result));
        }

        [Fact]
        public void HandleResult_ThrowsNotHandledException_WhenStatusIsCustomAndCustomHandlerIsNull()
        {
            // Arrange
            Result<string> result = new Result<string>((Status)1000);
            ResultHandler<string, string> resultHandler = new ResultHandler<string, string>()
                .OnSuccess(_ => "true")
                .OnClientError(_ => "false")
                .OnServerError(_ => "false");

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
            ResultHandler<string, string> resultHandler = new ResultHandler<string, string>()
                .OnSuccess(_ => "true")
                .OnClientError(_ => "false")
                .OnServerError(_ => "false")
                .OnCustomStatus(_ => "custom");

            // Act & Assert
            Assert.Throws<ReservedStatusException>(() => resultHandler.HandleResult(result));
        }

    }
}