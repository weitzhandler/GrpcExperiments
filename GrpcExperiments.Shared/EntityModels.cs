using System.Runtime.Serialization;
using System.ServiceModel;

namespace GrpcExperiments.Shared;

[ServiceContract]
public interface IEntityService<TEntity> where TEntity : class
{
    [OperationContract]
    Task<Entity?> GetById(EntityRequest<int> parameter);

    [OperationContract]
    Task<IEnumerable<Entity>> GetAll();

    [OperationContract]
    Task SaveAll(TEntity[] entities);
}

[DataContract]
public class EntityRequest<TParam>
{
    [DataMember(Order = 1)]
    public required TParam Param { get; set; }
}

[DataContract]
public class Entity
{
    [DataMember(Order = 1)]
    public required string Name { get; set; }
}
