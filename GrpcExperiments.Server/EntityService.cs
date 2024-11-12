

namespace GrpcExperiments.Server;

public class EntityService<TEntity> : IEntityService<TEntity>
    where TEntity : class
{
    public async Task<IEnumerable<Entity>> GetAll()
    {
        var collection = new[] {
            new Entity{ Name = "John"},
            new Entity{ Name = "Doe"}
        };
        return await Task.FromResult(collection);
    }

    public Task<Entity?> GetById(EntityRequest<int> parameter)
    {
        return Task.FromResult((Entity?)new Entity { Name = parameter.Param.ToString() });
    }

    public Task SaveAll(TEntity[] entities) => Task.CompletedTask;
}