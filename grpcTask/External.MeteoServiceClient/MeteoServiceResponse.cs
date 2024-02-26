namespace External.MeteoServiceClient;

public class MeteoServiceResponse
{
    public HourlyResults Hourly { get; set; }
}

public class HourlyResults
{
    public DateTime[] Time { get; set; }
    public double?[] Temperature_2m { get; set; }
}