namespace IFPlanner.Components.Classes;

public class ApiClient
{
    private readonly HttpClient _httpClient;

    public ApiClient()
    {
        _httpClient = new HttpClient();
    }

    public async Task<string> GetApiResponseAsString(string apiUrl)
    {
        try
        {
            var response = await _httpClient.GetAsync(apiUrl);
            response.EnsureSuccessStatusCode(); // Ensure success status code, throws exception otherwise
            return await response.Content.ReadAsStringAsync(); // Read response content as string
        }
        catch (HttpRequestException ex)
        {
            // Handle exception, log error, etc.
            Console.WriteLine($"An error occurred: {ex.Message}");
            return null;
        }
    }
}