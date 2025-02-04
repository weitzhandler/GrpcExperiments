using ProtoBuf.Grpc;
using System.ServiceModel;

namespace GrpcExperiments.Shared;

[ServiceContract]
public interface ITimeService
{
    [OperationContract]
    IAsyncEnumerable<TimeResult> SubscribeAsync(
        TimeRequest request,
        CallContext context = default);

    [OperationContract]
    IAsyncEnumerable<TimeIntervalResult> SubscribeIntervalAsync(
        TimeIntervalRequest request);
}

public record TimeRequest(int Interval);

public record TimeIntervalRequest(int Interval, int Duration);

public record TimeResult(DateTime Time);

public record TimeIntervalResult(Progress? Progress, Result? Result);

public record Progress(float Percent);

public record Result(string Value);