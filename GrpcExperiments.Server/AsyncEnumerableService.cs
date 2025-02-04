using GrpcExperiments.Shared;
using ProtoBuf.Grpc;

namespace GrpcExperiments.Server;

public class MyTimeService : ITimeService
{
    public async IAsyncEnumerable<TimeResult> SubscribeAsync(
        TimeRequest reqest,
        CallContext context = default)
    {
        var cancellationToken = context.CancellationToken;

        while (!cancellationToken.IsCancellationRequested)
        {
            await Task.Delay(reqest.Interval, cancellationToken);
            yield return new TimeResult(DateTime.UtcNow);
        }
        yield break;
    }
}