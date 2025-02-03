using Grpc.Core;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using ProtoBuf;
using ProtoBuf.Grpc;
using ProtoBuf.Grpc.ClientFactory;
using Gnc = Grpc.Net.ClientFactory;

namespace GrpcExperiements.Test;

public class AsyncEnumerableTests
{
    [Fact]
    public void Should_serialize_records()
    {
        // arrange
        var request = new TimeRequest(1);
        var result = new TimeResult(DateTime.UtcNow);

        // act
        request = Serializer.DeepClone(request);
        result = Serializer.DeepClone(result);

        // assert
    }

    [Fact]
    public async Task TestViaCodeFirst()
    {
        using var webApp = new WebApplicationFactory<Program>();

        var services = new ServiceCollection();
        services
            .AddCodeFirstGrpcClient<ITimeService>(clientFactoryOptions =>
            {
                clientFactoryOptions.Address = webApp.Server.BaseAddress;
                clientFactoryOptions.ChannelOptionsActions.Add(option => option.HttpHandler = webApp.Server.CreateHandler());
            });
        using var serviceProvider = services.BuildServiceProvider();
        var factory = serviceProvider.GetRequiredService<Gnc.GrpcClientFactory>();
        var client = factory.CreateClient<ITimeService>(nameof(ITimeService));

        var interval = 1;
        var duration = .3;
        var request = new TimeRequest(interval);
        var cancel = new CancellationTokenSource(TimeSpan.FromMinutes(duration));
        var options = new CallOptions(cancellationToken: cancel.Token);

        var results = new List<TimeResult>();

        try
        {
            await foreach (var time in client.SubscribeAsync(
                request, 
                new CallContext(options)))
            {
                results.Add(time);
            }
        }
        catch (RpcException ex) when (ex.StatusCode == StatusCode.Cancelled)
        {
        }
    }
}
