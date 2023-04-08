using EasyResults.Entities;
using EasyResults.Enums;
using EasyResults.Handlers;

namespace EasyResults.Tests
{
    public class DefinedStatusHandlerTests
    {
        [Fact]
        public void HandleResult_ReturnsResultHandlerResult_WhenStatusIsSuccessAndSuccessStatusHandlerIsDefined()
        {
            // Arrange
            Result<string> result = ResultsMock.Success;
            var resultHandler = new ResultHandler<string, string>()
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
            Result<string> result = ResultsMock.BadRequest;
            var resultHandler = new ResultHandler<string, string>()
                .OnSuccess(r => "success")
                .OnClientError(r => "client")
                .OnServerError(r => "server")
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
            Result<string> result = ResultsMock.Unauthorized;
            var resultHandler = new ResultHandler<string, string>()
                .OnSuccess(r => "success")
                .OnClientError(r => "client")
                .OnServerError(r => "server")
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
            Result<string> result = ResultsMock.Forbidden;
            var resultHandler = new ResultHandler<string, string>()
                .OnSuccess(r => "success")
                .OnClientError(r => "client")
                .OnServerError(r => "server")
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
            Result<string> result = ResultsMock.NotFound;
            var resultHandler = new ResultHandler<string, string>()
                .OnSuccess(r => "success")
                .OnClientError(r => "client")
                .OnServerError(r => "server")
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
            Result<string> result = ResultsMock.Conflict;
            var resultHandler = new ResultHandler<string, string>()
                .OnSuccess(r => "success")
                .OnClientError(r => "client")
                .OnServerError(r => "server")
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
            Result<string> result = ResultsMock.InternalServerError;
            var resultHandler = new ResultHandler<string, string>()
                .OnSuccess(r => "success")
                .OnClientError(r => "client")
                .OnServerError(r => "server")
                .OnStatus(Status.InternalServerError, r => "internalservererror");

            // Act
            string resultHandlerResult = resultHandler.HandleResult(result);

            // Assert
            Assert.Equal("internalservererror", resultHandlerResult);
        }
    }
}
