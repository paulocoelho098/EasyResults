using EasyResults.Entities;
using EasyResults.Enums;
using EasyResults.Handlers;

namespace EasyResults.Tests
{
    public class DefinedHandlersTests
    {
        [Fact]
        public void Execute_ReturnsResultHandlerResult_WhenActionIsDefined()
        {
            // Arrange
            ResultHandler<string, string> resultHandler = new ResultHandler<string, string>()
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
        public void HandleResult_ReturnsResultHandlerResult_WhenStatusIsError(Status status, string expected)
        {
            // Arrange
            Result<string> result = new(status);

            var resultHandler = new ResultHandler<string, string>()
            .OnSuccess(r => "success")
            .OnClientError(r => "client")
            .OnServerError(r => "server");

            // Act
            string resultHandlerResult = resultHandler.HandleResult(result);

            // Assert
            Assert.Equal(expected, resultHandlerResult);
        }

    }
}