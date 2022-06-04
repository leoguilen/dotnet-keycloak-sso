#nullable disable
namespace KeycloakDemo.Report.Api.Models;

public class OperationReportResponse
{
    public Guid OperationId { get; private init; }

    public Guid ExecutorId { get; private init; }

    public string Description { get; private init; }

    public DateTime ExecutedIn { get; private init; }

    public static OperationReportResponse From(OperationApiResponse apiResponse) => new()
    {
        OperationId = apiResponse.Id,
        ExecutorId = apiResponse.ExecutorId,
        Description = apiResponse.Description,
        ExecutedIn = apiResponse.ExecutedIn,
    };
}
