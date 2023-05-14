using EasyResults.Entities;
using EasyResults.Enums;

namespace EasyResults.Tests;

internal class ResultsMock
{
    internal static readonly Result Success = new(Status.Success, "Success");
    internal static readonly Result BadRequest = new(Status.BadRequest, "BadRequest");
    internal static readonly Result Unauthorized = new(Status.Unauthorized, "Unauthorized");
    internal static readonly Result Forbidden = new(Status.Forbidden, "Forbidden");
    internal static readonly Result NotFound = new(Status.NotFound, "NotFound");
    internal static readonly Result Conflict = new(Status.Conflict, "Conflict");
    internal static readonly Result InternalServerError = new(Status.InternalServerError, "InternalServerError");
}

internal class TestId
{
    public TestId(int id)
    {
        Id = id;
    }

    public int Id { get; set; }
}

