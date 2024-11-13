using System.Text.Json;
using IFPlanner.Components.Models;
using Microsoft.AspNetCore.Components;

namespace IFPlanner.Components.Pages;

public partial class ServerStats : ComponentBase
{
    private const string  ApiKey = Environment.GetEnvironmentVariable("ApiKey");
    private readonly List<string> _airplaneNames = new();
    private readonly List<AirplaneInfo> _airplaneValues = new();
    private List<InfiniteFlightApi.WorldStatus.AirportStatus> _combinedAirportStatusList = new();
    private bool _isInitialized;
    private List<InfiniteFlightApi.GetSessions.SessionInfo> _sessionInfos = new();

    [Inject] protected HttpClient Http { get; set; }


    protected override async Task OnInitializedAsync()
    {
        try
        {
            Http.DefaultRequestHeaders.Add("XApiKey", ApiKey);
            var sessionsUrl = "http://localhost:5000/world_ids";
            var sessionsTask = Http.GetFromJsonAsync<List<InfiniteFlightApi.GetSessions.SessionInfo>>(sessionsUrl);

            var jsonData = await File.ReadAllTextAsync("wwwroot/IfFlightIds.json");
            var ifFlightIds = JsonSerializer.Deserialize<List<IfFlightIdClass>>(jsonData);

            await Task.WhenAll(sessionsTask);

            var x = sessionsTask;

            if (x.Result != null)
            {
                _sessionInfos = x.Result;

                var worldStatusUrl = "http://localhost:5000/world_response";
                var flightsUrl = "http://localhost:5000/flights_response";

                _combinedAirportStatusList =
                    await Http.GetFromJsonAsync<List<InfiniteFlightApi.WorldStatus.AirportStatus>>(worldStatusUrl) ??
                    new List<InfiniteFlightApi.WorldStatus.AirportStatus>();
                var flightJson = await Http.GetFromJsonAsync<List<InfiniteFlightApi.Flights.FlightEntry>>(flightsUrl) ??
                                 new List<InfiniteFlightApi.Flights.FlightEntry>();

                foreach (var flight in flightJson)
                {
                    var aircraftName = ifFlightIds.FirstOrDefault(f => f.id == flight.AircraftId)?.name;
                    if (!string.IsNullOrEmpty(aircraftName))
                        _airplaneNames.Add(aircraftName);
                }

                var airplaneCounts = _airplaneNames.GroupBy(item => item).Select(group =>
                    new AirplaneInfo { Name = group.Key, AirplaneCount = group.Count() }).ToList();
                _airplaneValues.AddRange(airplaneCounts);
                _isInitialized = true;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("LOG: An error occurred: " + ex.Message);
        }
    }

    public class IfFlightIdClass
    {
        public string id { get; set; }
        public string name { get; set; }
    }

    public class AirplaneInfo
    {
        public string Name { get; set; }
        public int AirplaneCount { get; set; }
    }
}