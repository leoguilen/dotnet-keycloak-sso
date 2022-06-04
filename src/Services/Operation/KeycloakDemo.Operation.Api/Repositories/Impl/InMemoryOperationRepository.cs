namespace KeycloakDemo.Operation.Api.Repositories.Impl;

internal class InMemoryOperationRepository : IOperationRepository
{
    private Entities.Operation[] _operations;

    public InMemoryOperationRepository() => _operations = Array.Empty<Entities.Operation>();

    public IReadOnlyCollection<Entities.Operation> GetAll() => _operations;

    public Entities.Operation? GetById(Guid operationId) => Array.Find(_operations, op => op.Id == operationId);

    public void Insert(Entities.Operation operation)
    {
        Array.Resize(ref _operations, _operations.Length + 1);
        _operations[^1] = operation;
    }
}
