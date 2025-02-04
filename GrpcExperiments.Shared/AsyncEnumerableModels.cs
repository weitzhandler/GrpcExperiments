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
}

public record TimeRequest(int Interval);
public record TimeResult(DateTime Time);