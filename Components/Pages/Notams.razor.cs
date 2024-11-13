using IFPlanner.Components.Models;
using Microsoft.AspNetCore.Components;

namespace IFPlanner.Components.Pages;

public partial class Notams : ComponentBase
{
    private bool _isInitialized;
    private List<InfiniteFlightApi.Notams.NotamResult> _notamResults = new();
    private const string  ApiKey = Environment.GetEnvironmentVariable("ApiKey");
    [Inject] protected HttpClient Http { get; set; }

    protected override async Task OnInitializedAsync()
    {
        try
        {
            _isInitialized = false;
            Http.DefaultRequestHeaders.Add("XApiKey", ApiKey);
            var notamsUrl = "http://localhost:5000/notams_response";
            _notamResults  = await Http.GetFromJsonAsync<List<InfiniteFlightApi.Notams.NotamResult>>(notamsUrl);
            _isInitialized = true;
        }
        catch (Exception ex)
        {
            Console.WriteLine("LOG: An error occurred: " + ex.Message);
        }
    }
}