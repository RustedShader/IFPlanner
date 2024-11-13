using IFPlanner.Components.Models;
using Microsoft.AspNetCore.Components;

namespace IFPlanner.Components.Pages;




public partial class Atis
{
    private List<string> _atisResponse = new();
    private bool _isInitialized;
    private const string  ApiKey = Environment.GetEnvironmentVariable("ApiKey");
    [Inject] protected HttpClient Http { get; set; }

    protected override async Task OnInitializedAsync()
    {
        try
        {
            _isInitialized = false;
            Http.DefaultRequestHeaders.Add("XApiKey", ApiKey);
            var atisUrl = "http://localhost:5000/atis_response";
            _atisResponse = await Http.GetFromJsonAsync<List<string>>(atisUrl);
            _isInitialized = true;
        }
        catch (Exception ex)
        {
            Console.WriteLine("LOG: An error occurred: " + ex.Message);
        }
    }

    
}