namespace KeycloakDemo.Report.Api.Infra.Services.Impl;

internal class OperationApiService : IOperationApiService
{
    private readonly IHttpClientFactory _httpClientFactory;

    public OperationApiService(IHttpClientFactory httpClientFactory) => _httpClientFactory = httpClientFactory;

    public async Task<IReadOnlyList<OperationApiResponse>> GetOperationsAsync(CancellationToken cancellationToken = default)
    {
        using var httpClient = _httpClientFactory.CreateClient("OperationApiClient");

        var response = await httpClient
            .GetFromJsonAsync<IReadOnlyList<OperationApiResponse>>("/api/operations", cancellationToken);

        return response ?? Array.Empty<OperationApiResponse>();
    }
}
