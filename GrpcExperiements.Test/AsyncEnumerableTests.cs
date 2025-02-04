using Grpc.Core;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using ProtoBuf.Grpc;
using ProtoBuf.Grpc.ClientFactory;
using Gnc = Grpc.Net.ClientFactory;

namespace GrpcExperiements.Test;

public class AsyncEnumerableTests
{
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

        var interval = 10;
        var duration = 1000;
        var request = new TimeRequest(interval);
        var results = new List<TimeResult>();
        var cancel = new CancellationTokenSource(duration);
        var options = new CallOptions(cancellationToken: cancel.Token);

        try
        {
            await foreach (var time in client.SubscribeAsync(request, new CallContext(options)))
            {
                results.Add(time);
            }
        }
        catch (RpcException ex) when (ex.StatusCode == StatusCode.Cancelled) { /* Thrown when client call cancelled */ }
        catch (OperationCanceledException ex) when (ex.CancellationToken.IsCancellationRequested) { /* can be thrown from the Task.Delay */ }

        Assert.True(results.Count > 0);
    }
}
