using Grpc.Net.Client;
using Microsoft.Extensions.DependencyInjection;
using ProtoBuf.Grpc.Client;
using ProtoBuf.Grpc.ClientFactory;
using Shared.Contracts;
using Gncf = Grpc.Net.ClientFactory;

public class Program
{
    public static async Task Main()
    {
        await CreateClientNew();
    }

    static async Task CreateClientNew()
    {
        var services = new ServiceCollection();
        services.AddCodeFirstGrpcClient<IGreeterService>(o =>
        {
            o.Address = new Uri("https://localhost:7158");
        });
        var serviceProvider = services.BuildServiceProvider();
        var factory = serviceProvider.GetRequiredService<Gncf.GrpcClientFactory>();
        var client = factory.CreateClient<IGreeterService>(nameof(IGreeterService));

        var reply = await client.SayHelloAsync(
            new HelloRequest { Name = "GreeterClient" });

        Console.WriteLine($"Greeting: {reply.Message}");
        Console.WriteLine("Press any key to exit...");
        Console.ReadKey();
    }

    static async Task CreateClientOld()
    {
        using var channel = GrpcChannel.ForAddress("https://localhost:7158");
        var client = channel.CreateGrpcService<IGreeterService>();

        var reply = await client.SayHelloAsync(
            new HelloRequest { Name = "GreeterClient" });

        Console.WriteLine($"Greeting: {reply.Message}");
        Console.WriteLine("Press any key to exit...");
        Console.ReadKey();
    }
}