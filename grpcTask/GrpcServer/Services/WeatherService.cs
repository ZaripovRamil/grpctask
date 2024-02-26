using External.MeteoServiceClient;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Measure;

namespace GrpcServer.Services;

public class WeatherService : Measure.WeatherService.WeatherServiceBase
{
    private readonly MeteoMeasureServiceClient _client = new();
    public override async Task WeatherStream(Request request, IServerStreamWriter<Response> responseStream, ServerCallContext context)
    {
        var measures = _client.GetMeteoMeasures();
        await foreach (var measure in measures)
        {
            await responseStream.WriteAsync(new Response { CelsiusDegrees = measure.CelsiusDegrees.Value, MeasureTime = measure.MeasureTime.ToUniversalTime().ToTimestamp()});
            await Task.Delay(TimeSpan.FromSeconds(1));
            if(context.CancellationToken.IsCancellationRequested)
                break;
        }
    }
}