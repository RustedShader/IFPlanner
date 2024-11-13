using System.Text.Json;
using IFPlanner.Components.Models;
using Microsoft.AspNetCore.Components;

namespace IFPlanner.Components.Pages;

public partial class UserStats : ComponentBase
{
    private string _userName = string.Empty;
    private const string  ApiKey = Environment.GetEnvironmentVariable("InfiniteFlightApiKey");

    private readonly List<InfiniteFlightApi.UserStatsApi.UserStats> _userStats = new();

    private List<InfiniteFlightApi.UserFlightData.UserFlight> _userFlights = new();

    private bool _isInitialized = true;
    [Inject] private HttpClient HttpClient { get; set; }

    private async Task GetUserFlights()
    {
        var userFlightUrl = $"https://api.infiniteflight.com/public/v2/users/{_userStats.FirstOrDefault()?.userId}/flights?apikey=" + ApiKey;
        var x = await HttpClient.GetAsync(userFlightUrl);
        if (x.IsSuccessStatusCode)
        {
            var userFlightData =
                await HttpClient.GetFromJsonAsync<InfiniteFlightApi.UserFlightData.LiveAPIResponse>(userFlightUrl);
            _userFlights = userFlightData.result.data;
        }
    }

    private async Task GetUserData()
    {
        _isInitialized = false;   
        var data = new { discourseNames = new[] { _userName } };
        // var data = $"{{\"discourseNames\": [ \"RustedShader\" ]}}";
        Console.WriteLine(data);
        var userStatsUrl = "https://api.infiniteflight.com/public/v2/users?apikey=" + ApiKey;
        var jsonData = await HttpClient.PostAsJsonAsync(userStatsUrl, data);
        if (jsonData.IsSuccessStatusCode)
        {
            var response = await jsonData.Content.ReadAsStringAsync();
            if (!string.IsNullOrEmpty(response))
            {
                var x = JsonSerializer.Deserialize<InfiniteFlightApi.UserStatsApi.LiveAPIResponse>(response);
                _userStats.Clear();
                _userStats.AddRange(x.result);
                await GetUserFlights();
                _isInitialized = true;
            }
        }
        _isInitialized = true;
    }
}