namespace KeycloakDemo.Operation.Api;

internal static class EndpointHandlers
{
    public static readonly Func<IOperationService, HttpContext, Task<IResult>> GetOperationsHandler = HandleGetOperationsAsync;
    public static readonly Func<Guid, IOperationService, Task<IResult>> GetOperationHandler = HandleGetOperationAsync;
    public static readonly Func<CreateOperationRequest, IOperationService, HttpContext, Task<IResult>> CreateOperationHandler = HandleCreateOperationAsync;

    private static async Task<IResult> HandleGetOperationsAsync(
        [FromServices] IOperationService operationService,
        HttpContext context)
        => Results.Ok(await operationService.GetOperationsAsync());

    private static async Task<IResult> HandleGetOperationAsync(
        [FromRoute] Guid operationId,
        [FromServices] IOperationService operationService)
        => await operationService.GetOperationAsync(operationId) switch
        {
            var response when response.HasValue => Results.Ok(response),
            _ => Results.NotFound(),
        };

    private static async Task<IResult> HandleCreateOperationAsync(
        [FromBody] CreateOperationRequest request,
        [FromServices] IOperationService operationService,
        HttpContext context)
    {
        var userId = context.User.Claims
            .First(c => c.Type == ClaimTypes.NameIdentifier)
            .Value;

        request.SetExecutor(Guid.Parse(userId!));

        var createdOperationId = await operationService.CreateNewAsync(request);
        return Results.Created(
            uri: $"/api/operations/{createdOperationId}",
            value: new
            {
                OperationId = createdOperationId,
            });
    }
}
