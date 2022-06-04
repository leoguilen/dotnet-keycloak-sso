#nullable disable
namespace KeycloakDemo.Report.Api.Models;

public record OperationApiResponse
{
    [JsonPropertyName("id")]
    public Guid Id { get; init; }

    [JsonPropertyName("executorId")]
    public Guid ExecutorId { get; init; }

    [JsonPropertyName("description")]
    public string Description { get; init; }

    [JsonPropertyName("executedIn")]
    public DateTime ExecutedIn { get; init; }
}
