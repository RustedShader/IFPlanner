using System.Text.Json;
using IFPlanner.Components.Models;

namespace IFPlanner.Components.Pages;

public partial class AirplaneStats
{
    private List<AircraftPerformance> _aircraftList = new();
    private bool _isInitialized;

    protected override async Task OnInitializedAsync()
    {
        _isInitialized = false;
        var jsonData = await File.ReadAllTextAsync("wwwroot/AircraftPerformance.json");
        _aircraftList = JsonSerializer.Deserialize<List<AircraftPerformance>>(jsonData);
        _isInitialized = true;
    }
}