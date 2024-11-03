using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using ProtoBuf.Grpc.ClientFactory;
using Gncf = Grpc.Net.ClientFactory;

namespace GrpcExperiements.Test;
public class EntityServiceTests
{

    [Fact]
    public async Task TestGetById()
    {
        using var webApp = new WebApplicationFactory<Program>();

        var services = new ServiceCollection();
        services
            .AddCodeFirstGrpcClient<IEntityService<Entity>>(nameof(IEntityService<Entity>), clientFactoryOptions =>
            {
                clientFactoryOptions.Address = webApp.Server.BaseAddress;
                clientFactoryOptions.ChannelOptionsActions.Add(option => option.HttpHandler = webApp.Server.CreateHandler());
            });

        using var serviceProvider = services.BuildServiceProvider();
        var factory = serviceProvider.GetRequiredService<Gncf.GrpcClientFactory>();
        var client = factory.CreateClient<IEntityService<Entity>>(nameof(IEntityService<Entity>));

        var reply = await client.GetById(
            new EntityRequest<int> { Param = 47 });

        Assert.Equal("47", reply?.Name);
    }

    [Fact]
    public async Task TestGetAll()
    {
        using var webApp = new WebApplicationFactory<Program>();

        var services = new ServiceCollection();
        services
            .AddCodeFirstGrpcClient<IEntityService<Entity>>(nameof(IEntityService<Entity>), clientFactoryOptions =>
            {
                clientFactoryOptions.Address = webApp.Server.BaseAddress;
                clientFactoryOptions.ChannelOptionsActions.Add(option => option.HttpHandler = webApp.Server.CreateHandler());
            });

        using var serviceProvider = services.BuildServiceProvider();
        var factory = serviceProvider.GetRequiredService<Gncf.GrpcClientFactory>();
        var client = factory.CreateClient<IEntityService<Entity>>(nameof(IEntityService<Entity>));

        var reply = await client.GetAll();
        Assert.Equal(2, reply.Count());
    }


}
