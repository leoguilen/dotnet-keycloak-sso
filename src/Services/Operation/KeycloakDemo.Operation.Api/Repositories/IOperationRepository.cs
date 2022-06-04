namespace KeycloakDemo.Operation.Api.Repositories;

public interface IOperationRepository
{
    IReadOnlyCollection<Entities.Operation> GetAll();

    Entities.Operation? GetById(Guid operationId);

    void Insert(Entities.Operation operation);
}
