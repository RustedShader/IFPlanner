using System.Runtime.InteropServices;
using System.Text.Json;
using IFPlanner.Components.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Radzen;

namespace IFPlanner.Components.Pages;

public partial class RoutesToFly : ComponentBase
{
    private readonly List<ToFlyRoutesClass.BusyRoute> _busyRoutes = new();
    private readonly List<ToFlyRoutesClass.LongHaulScenicRoute> _longHaulScenicRoutes = new();
    private readonly List<ToFlyRoutesClass.MediumHaulScenicRoute> _mediumHaulScenicRoutes = new();
    private readonly List<ToFlyRoutesClass.ShortHaulScenicRoute> _shortHaulScenicRoutes = new();

    private bool _isInitialized;

    private IList<ToFlyRoutesClass.BusyRoute> _selectedBusiestRoute;
    private IList<ToFlyRoutesClass.LongHaulScenicRoute> _selectedLongHaulScenicRoutesRoute;
    private IList<ToFlyRoutesClass.ShortHaulScenicRoute> _selectedShortHaulScenicRoutesRoute;
    private IList<ToFlyRoutesClass.MediumHaulScenicRoute> _selecteMediumHaulScenicRoutesRoute;

    [Inject] protected IJSRuntime JsRuntime { get; set; }
    [Inject] protected NavigationManager NavigationManager { get; set; }
    [Inject] protected DialogService DialogService { get; set; }
    [Inject] protected TooltipService TooltipService { get; set; }
    [Inject] protected ContextMenuService ContextMenuService { get; set; }
    [Inject] protected NotificationService NotificationService { get; set; }
    [Inject] protected HttpClient HttpClient { get; set; }

    private void NavigateToPage(string originIcao, string destinationIcao, string aircraftModel)
    {
        NavigationManager.NavigateTo(
            $"/create-flight-plan/?aircraftModel={aircraftModel}&departureAirport={originIcao}&arrivalAirport={destinationIcao}");
    }

    protected override async Task OnInitializedAsync()
    {
        _isInitialized = false;
        var y = await File.ReadAllTextAsync("wwwroot/RoutesData.json");
        var x = JsonSerializer.Deserialize<ToFlyRoutesClass.RootObject>(y);
        _shortHaulScenicRoutes.AddRange(x.shortHaulScenicRoutes);
        _mediumHaulScenicRoutes.AddRange(x.mediumHaulScenicRoutes);
        _longHaulScenicRoutes.AddRange(x.longHaulScenicRoutes);
        _busyRoutes.AddRange(x.busiestRoutes);
        Random.Shared.Shuffle(CollectionsMarshal.AsSpan(_shortHaulScenicRoutes));
        Random.Shared.Shuffle(CollectionsMarshal.AsSpan(_mediumHaulScenicRoutes));
        Random.Shared.Shuffle(CollectionsMarshal.AsSpan(_longHaulScenicRoutes));
        Random.Shared.Shuffle(CollectionsMarshal.AsSpan(_busyRoutes));
        _isInitialized = true;
    }
}