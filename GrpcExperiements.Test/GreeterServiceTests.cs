using Grpc.Net.Client;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using ProtoBuf.Grpc.Client;
using ProtoBuf.Grpc.ClientFactory;
using Shared.Contracts;
using Xunit.Abstractions;
using Gncf = Grpc.Net.ClientFactory;

namespace GrpcExperiements.Test;

public class GreeterServiceTests
{
    private readonly ILoggerFactory _loggerFactory;

    public GreeterServiceTests(ITestOutputHelper testOutputHelper)
    {
        _loggerFactory = LoggerFactory.Create(l =>
        {
            l.AddProvider(new XunitLoggerProvider(testOutputHelper));
        });
    }

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
        using var webApp = new WebApplicationFactory<Program>()
            .WithWebHostBuilder(builder =>
            {
                builder.ConfigureServices(services =>
                {
                    services.AddSingleton(_loggerFactory);
                });
            });

        var services = new ServiceCollection();
        services.AddCodeFirstGrpcClient<IGreeterService>(o =>
        {
            o.Address = webApp.Server.BaseAddress;
        })
        .ConfigureChannel(channelOptions =>
        {
            channelOptions.LoggerFactory = _loggerFactory;
        });

        var serviceProvider = services.BuildServiceProvider();
        var factory = serviceProvider.GetRequiredService<Gncf.GrpcClientFactory>();
        var client = factory.CreateClient<IGreeterService>(nameof(IGreeterService));

        var reply = await client.SayHelloAsync(
            new HelloRequest { Name = "GreeterClient" });

        Assert.Equal("Hello GreeterClient", reply.Message);
    }
}