namespace FTELMarket.Infrastructure.Services;

public class FlakyService
{
    private readonly HttpClient _httpClient;

    // httpclient déjà paramètré grace à program.cs
    public FlakyService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<bool> IsHealthyAsync()
    {
        var response = await _httpClient.GetAsync("health");
        return response.IsSuccessStatusCode;
    }
}