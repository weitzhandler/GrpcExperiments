using ProtoBuf;
using System.Runtime.Serialization;
using System.ServiceModel;

namespace GrpcExperiments.Shared;

[ServiceContract]
public interface IPolymorphismService
{
    [OperationContract]
    public ValueTask<ConcreteClass> GetConcreteClass();
}

[DataContract]
[ProtoInclude(2, typeof(ConcreteClass))]
public abstract class BaseClass
{
    [DataMember(Order = 1)]
    public int BaseClassProperty { get; set; }
}

[DataContract]
public class ConcreteClass : BaseClass
{
    [DataMember(Order = 1)]
    public int ConcreteClassProperty { get; set; }
}