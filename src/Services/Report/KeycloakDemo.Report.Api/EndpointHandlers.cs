namespace KeycloakDemo.Report.Api;

internal static class EndpointHandlers
{
    public static readonly Func<IOperationApiService, CancellationToken, Task<IResult>> GetOperationsReportHandler = HandleGetOperationsAsync;

    private static async Task<IResult> HandleGetOperationsAsync(
        [FromServices] IOperationApiService operationApiService,
        CancellationToken cancellationToken = default)
    {
        var apiResponse = await operationApiService
            .GetOperationsAsync(cancellationToken);

        return Results.Ok(apiResponse.Select(res => OperationReportResponse.From(res)));
    }
}
