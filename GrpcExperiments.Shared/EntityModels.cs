using System.Runtime.Serialization;
using System.ServiceModel;

[ServiceContract]
public interface IEntityService<TEntity> where TEntity : class
{
    [OperationContract]
    Task<Entity?> GetById(EntityRequest<int> parameter);

    [OperationContract]
    Task<IEnumerable<Entity>> GetAll();
}

[DataContract]
public class EntityRequest<TParam>
{
    [DataMember(Order = 1)]
    public TParam? Param { get; set; }
}

[DataContract]
public class Entity
{
    [DataMember(Order = 1)]
    public string? Name { get; set; }

}