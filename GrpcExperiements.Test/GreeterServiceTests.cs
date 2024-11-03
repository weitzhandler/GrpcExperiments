using Grpc.Net.Client;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using ProtoBuf.Grpc.Client;
using ProtoBuf.Grpc.ClientFactory;
using Shared.Contracts;
using Gncf = Grpc.Net.ClientFactory;

namespace GrpcExperiements.Test;

public class GreeterServiceTests
{
    [Fact]
    public async Task TestViaGrpcChannel()
    {
        using var factory = new WebApplicationFactory<Program>();

        using var channel = GrpcChannel.ForAddress(factory.Server.BaseAddress, new GrpcChannelOptions
        {            
            HttpClient = factory.CreateClient()
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
            .AddCodeFirstGrpcClient<IGreeterService>(nameof(IGreeterService), clientFactoryOptions =>
            {
                clientFactoryOptions.Address = webApp.Server.BaseAddress;
                //clientFactoryOptions.ChannelOptionsActions.Add(options => options.HttpClient = webApp.CreateClient());
            });
            //.ConfigureChannel(channel => 
            //    channel.HttpClient = webApp.CreateClient());

        var serviceProvider = services.BuildServiceProvider();
        var factory = serviceProvider.GetRequiredService<Gncf.GrpcClientFactory>();
        var client = factory.CreateClient<IGreeterService>(nameof(IGreeterService));

        var reply = await client.SayHelloAsync(
            new HelloRequest { Name = "GreeterClient" });

        Assert.Equal("Hello GreeterClient", reply.Message);
    }
}