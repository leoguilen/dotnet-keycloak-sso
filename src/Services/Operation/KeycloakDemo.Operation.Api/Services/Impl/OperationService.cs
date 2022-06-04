namespace KeycloakDemo.Operation.Api.Services.Impl;

internal class OperationService : IOperationService
{
    private readonly IOperationRepository _operations;

    public OperationService(IOperationRepository operations) => _operations = operations;

    public async ValueTask<Guid> CreateNewAsync(CreateOperationRequest request)
    {
        var newOperation = request.ToEntity();
        _operations.Insert(newOperation);
        return await ValueTask.FromResult(newOperation.Id);
    }

    public async ValueTask<OperationResponse?> GetOperationAsync(Guid operationId)
        => await ValueTask
            .FromResult<OperationResponse?>(
                _operations.GetById(operationId) switch
                {
                    var operation when operation is not null => OperationResponse.From(operation),
                    _ => null,
                });

    public async ValueTask<OperationResponse[]> GetOperationsAsync()
        => await ValueTask
            .FromResult(_operations.GetAll() switch
            {
                var operations when operations is { Count: > 0 } => operations
                    .Select(op => OperationResponse.From(op))
                    .ToArray(),
                _ => Array.Empty<OperationResponse>(),
            });
}
