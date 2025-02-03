using ProtoBuf.Grpc;

public class MyTimeService : ITimeService
{
    public async IAsyncEnumerable<TimeResult> SubscribeAsync(
        TimeRequest reqest,
        CallContext context = default)
    {
        var cancellationToken = context.CancellationToken;

        var interval = TimeSpan.FromSeconds(reqest.Interval);

        while (!cancellationToken.IsCancellationRequested)
        {
            await Task.Delay(interval, cancellationToken);
            yield return new TimeResult(DateTime.UtcNow);
        }

    }
}