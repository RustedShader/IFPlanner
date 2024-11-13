using IFPlanner.Components.Models;
using Microsoft.AspNetCore.Components;

namespace IFPlanner.Components.Pages;

public partial class AirportStats
{
    private readonly List<InfiniteFlightApi.Airports.AirportInfo> _airportInfos = new();
    private string _airportIcao = string.Empty;
    private bool _isInitialized = true;
    [Inject] protected HttpClient Http { get; set; }

    private void HandleInput(ChangeEventArgs args)
    {
        var value = args.Value?.ToString()?.ToUpper();
        if (!string.IsNullOrEmpty(value) && value.Length == 4) _airportIcao = value;
    }

    protected async Task GetAirportsData()
    {
        try
        {
            _isInitialized = false;
            private const string  ApiKey = Environment.GetEnvironmentVariable("InfiniteFlightApiKey");
            var airportUrl = $"https://api.infiniteflight.com/public/v2/airport/{_airportIcao.ToUpper()}?apikey=" +
                             apiKey;

            var responseMessage = await Http.GetAsync(airportUrl);

            if (responseMessage.IsSuccessStatusCode)
            {
                var x = await Http.GetFromJsonAsync<InfiniteFlightApi.Airports.LiveAPIResponse>(airportUrl) ?? new InfiniteFlightApi.Airports.LiveAPIResponse();
                _airportInfos.Clear();
                _airportInfos.Add(x.Result);
                _isInitialized = true;
            }
            _isInitialized = true;
        }
        catch (Exception ex)
        {
            Console.WriteLine("LOG: An error occurred: " + ex.Message);
        }
    }
}