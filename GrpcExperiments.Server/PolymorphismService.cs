namespace GrpcExperiments.Server;

public class PolymorphismService : IPolymorphismService
{
    public ValueTask<ConcreteClass> GetConcreteClass()
    {
        var result = new ConcreteClass { BaseClassProperty = 1, ConcreteClassProperty = 2 };
        return new(result);
    }
}
