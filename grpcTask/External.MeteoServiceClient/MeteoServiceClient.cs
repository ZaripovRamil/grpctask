using System.Net.Http.Json;
using System.Text.Json;

namespace External.MeteoServiceClient;

public class MeteoMeasureServiceClient
{
    public async IAsyncEnumerable<MeteoMeasure> GetMeteoMeasures()
    {
        using var client = new HttpClient();
        client.BaseAddress =
            new Uri("https://archive-api.open-meteo.com/v1/");
        var response =
            await client.GetAsync(
                "archive?latitude=52.52&longitude=13.41&start_date=2024-01-01&end_date=2024-02-24&hourly=temperature_2m&timezone=Europe%2FMoscow");
        var parsedResponse = await response.Content.ReadFromJsonAsync<MeteoServiceResponse>(
            new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        for (var i = 0; i < parsedResponse.Hourly.Time.Length; i+=2)
            yield return new MeteoMeasure
            {
                CelsiusDegrees = parsedResponse.Hourly.Temperature_2m[i],
                MeasureTime = parsedResponse.Hourly.Time[i]
            };
    }
}