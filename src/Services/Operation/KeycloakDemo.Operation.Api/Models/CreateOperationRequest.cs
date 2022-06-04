namespace KeycloakDemo.Operation.Api.Models;

public record struct CreateOperationRequest
{
    [JsonIgnore]
    public Guid ExecutorId { get; private init; }

    public string Description { get; init; }

    public void SetExecutor(Guid executorId)
    {
        this = this with
        {
            ExecutorId = executorId,
        };
    }

    public Entities.Operation ToEntity() => new(ExecutorId, Description);
}
