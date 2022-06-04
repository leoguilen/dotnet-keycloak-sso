namespace KeycloakDemo.Operation.Api.Services;

public interface IOperationService
{
    ValueTask<OperationResponse[]> GetOperationsAsync();

    ValueTask<OperationResponse?> GetOperationAsync(Guid operationId);

    ValueTask<Guid> CreateNewAsync(CreateOperationRequest request);
}
