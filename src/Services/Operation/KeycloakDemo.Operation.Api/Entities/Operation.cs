#nullable disable
namespace KeycloakDemo.Operation.Api.Entities;

public record Operation
{
    public Operation(Guid executorId, string operationDescription)
    {
        Id = Guid.NewGuid();
        ExecutorId = executorId;
        Description = operationDescription;
        ExecutedIn = DateTime.Now;
    }

    public Guid Id { get; init; }

    public Guid ExecutorId { get; init; }

    public string Description { get; init; }

    public DateTime ExecutedIn { get; init; }
}
