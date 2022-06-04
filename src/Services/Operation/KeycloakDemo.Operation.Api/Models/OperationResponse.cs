namespace KeycloakDemo.Operation.Api.Models;

public readonly record struct OperationResponse
{
    public Guid Id { get; init; }

    public Guid ExecutorId { get; init; }

    public string Description { get; init; }

    public DateTime ExecutedIn { get; init; }

    public static OperationResponse From(Entities.Operation operation) => new()
    {
        Id = operation.Id,
        ExecutorId = operation.ExecutorId,
        Description = operation.Description,
        ExecutedIn = operation.ExecutedIn,
    };
}
