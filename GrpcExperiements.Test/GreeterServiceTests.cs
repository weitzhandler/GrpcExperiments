using Grpc.Net.Client;
using GrpcExperiments.Shared;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using ProtoBuf.Grpc.Client;
using ProtoBuf.Grpc.ClientFactory;
using Gnc = Grpc.Net.ClientFactory;

namespace GrpcExperiements.Test;

public class GreeterServiceTests
{
    [Fact]
    public async Task TestViaGrpcChannel()
    {
        using var webApp = new WebApplicationFactory<Program>();

        using var channel = GrpcChannel.ForAddress(webApp.Server.BaseAddress, new GrpcChannelOptions
        {
            HttpClient = webApp.CreateClient()
        });

        var client = channel.CreateGrpcService<IGreeterService>();

        var reply = await client.SayHelloAsync(
            new HelloRequest { Name = "GreeterClient" });

        Assert.Equal("Hello GreeterClient", reply.Message);
    }

    [Fact]
    public async Task TestViaCodeFirst()
    {
        using var webApp = new WebApplicationFactory<Program>();

        var services = new ServiceCollection();
        services
            .AddCodeFirstGrpcClient<IGreeterService>(clientFactoryOptions =>
            {
                clientFactoryOptions.Address = webApp.Server.BaseAddress;
                clientFactoryOptions.ChannelOptionsActions.Add(option => option.HttpHandler = webApp.Server.CreateHandler());
            });

        using var serviceProvider = services.BuildServiceProvider();
        var factory = serviceProvider.GetRequiredService<Gnc.GrpcClientFactory>();
        var client = factory.CreateClient<IGreeterService>(nameof(IGreeterService));

        var reply = await client.SayHelloAsync(
            new HelloRequest { Name = "GreeterClient" });

        Assert.Equal("Hello GreeterClient", reply.Message);
    }
}