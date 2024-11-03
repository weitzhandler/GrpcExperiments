using Grpc.Net.Client;
using Microsoft.AspNetCore.Mvc.Testing;
using ProtoBuf.Grpc.Client;

namespace GrpcExperiements.Test;
public class EntityServiceTests
{
    [Fact]
    public async Task TestGetById()
    {
        using var factory = new WebApplicationFactory<Program>();

        using var channel = GrpcChannel.ForAddress(factory.Server.BaseAddress, new GrpcChannelOptions
        {
            HttpClient = factory.CreateClient()
        });

        var client = channel.CreateGrpcService<IEntityService<Entity>>();

        var par = new EntityRequest<int> { Param = 47 };
        var reply = await client.GetById(par);

        Assert.Equal("47", reply?.Name);
    }

        [Fact]
    public async Task TestGetAll()
    {
        using var factory = new WebApplicationFactory<Program>();

        using var channel = GrpcChannel.ForAddress(factory.Server.BaseAddress, new GrpcChannelOptions
        {
            HttpClient = factory.CreateClient()
        });

        var client = channel.CreateGrpcService<IEntityService<Entity>>();

        var reply = await client.GetAll();
    }


}
