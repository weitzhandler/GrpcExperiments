using Grpc.Net.Client;
using GrpcExperiments.Shared;
using Microsoft.AspNetCore.Mvc.Testing;
using ProtoBuf.Grpc.Client;

namespace GrpcExperiements.Test;

public class PolymorphismTests
{
    [Fact]
    public async Task Test_using_gRPC_channel()
    {
        using var webApp = new WebApplicationFactory<Program>();

        using var channel = GrpcChannel.ForAddress(webApp.Server.BaseAddress, new GrpcChannelOptions
        {
            HttpClient = webApp.CreateClient()
        });

        var client = channel.CreateGrpcService<IPolymorphismService>();

        var reply = await client.GetConcreteClass();

        Assert.Equal(1, reply.BaseClassProperty);
        Assert.Equal(2, reply.ConcreteClassProperty);
    }
}