using GrpcExperiments.Shared;
using ProtoBuf.Grpc;

namespace GrpcExperiments.Server;

public class MyTimeService : ITimeService
{
    public async IAsyncEnumerable<TimeResult> SubscribeAsync(
        TimeRequest request,
        CallContext context = default)
    {
        var cancellationToken = context.CancellationToken;

        while (!cancellationToken.IsCancellationRequested)
        {
            await Task.Delay(request.Interval, cancellationToken);
            yield return new TimeResult(DateTime.UtcNow);
        }
        yield break;
    }

    public async IAsyncEnumerable<TimeIntervalResult> SubscribeIntervalAsync(TimeIntervalRequest request)
    {
        var duration = request.Duration;
        var cancel = new CancellationTokenSource(duration);
        var cancellationToken = cancel.Token;

        var current = 0f;
        while (!cancellationToken.IsCancellationRequested)
        {
            await Task.Delay(request.Interval);

            current++;
            var currentProgress = current * request.Interval / duration;
            yield return new TimeIntervalResult(new Progress(currentProgress), new Result(DateTime.UtcNow.ToShortTimeString()));
        }
        yield break;
    }
}