using EasyResults.Entities;
using EasyResults.Enums;

namespace EasyResults.Tests
{
    internal class ResultsMock
    {
        internal static readonly Result<string> Success = new(Status.Success, "Success");
        internal static readonly Result<string> BadRequest = new(Status.BadRequest, "BadRequest");
        internal static readonly Result<string> Unauthorized = new(Status.Unauthorized, "Unauthorized");
        internal static readonly Result<string> Forbidden = new(Status.Forbidden, "Forbidden");
        internal static readonly Result<string> NotFound = new(Status.NotFound, "NotFound");
        internal static readonly Result<string> Conflict = new(Status.Conflict, "Conflict");
        internal static readonly Result<string> InternalServerError = new(Status.InternalServerError, "InternalServerError");
    }
}
