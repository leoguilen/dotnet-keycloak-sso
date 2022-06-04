namespace KeycloakDemo.Report.Api.Infra.Services;

public interface IOperationApiService
{
    Task<IReadOnlyList<OperationApiResponse>> GetOperationsAsync(CancellationToken cancellationToken = default);
}
