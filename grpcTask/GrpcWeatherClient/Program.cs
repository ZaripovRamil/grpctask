using Grpc.Core;
using Grpc.Net.Client;
using Measure;

using var channel = GrpcChannel.ForAddress("http://localhost:9999");

var client = new WeatherService.WeatherServiceClient(channel);

var serverData = client.WeatherStream(new Request());

var responseStream = serverData.ResponseStream;

var cancellationTokenSource = new CancellationTokenSource();

Task.Run(() =>
{
    Thread.Sleep(TimeSpan.FromSeconds(5));
    cancellationTokenSource.Cancel();
});

responseStream.MoveNext();
while (true)
{
    Thread.Sleep(TimeSpan.FromSeconds(1));
    var response = responseStream.Current;
    Console.WriteLine($"{response.MeasureTime} : {response.CelsiusDegrees}");
    if (cancellationTokenSource.Token.IsCancellationRequested)
        break;
    responseStream.MoveNext();
}

Console.WriteLine("Cancellation token has been cancelled"); 