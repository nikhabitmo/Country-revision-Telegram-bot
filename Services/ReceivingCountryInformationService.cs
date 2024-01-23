using CountriesCapitalTelegramBot.Entities;
using CountriesCapitalTelegramBot.Models;
using Newtonsoft.Json;

namespace CountriesCapitalTelegramBot.Services;

public class ReceivingCountryInformationService
{
    private string _api;

    public ReceivingCountryInformationService(string api)
    {
        _api = api;
    }

    public async Task<List<CountryInformation>?> GetCountries()
    {
        using var client = new HttpClient();
        try
        {
            HttpResponseMessage response = await client.GetAsync(_api);

            if (response.IsSuccessStatusCode)
            {
                string content = await response.Content.ReadAsStringAsync();
                // Console.WriteLine(content);
                List<CountryInformation> countries = JsonConvert.DeserializeObject<List<CountryInformation>>(content);
                return countries;
            }
            else
            {
                Console.WriteLine($"Error: {response.StatusCode}");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Exception: {ex.Message}");
        }

        return new List<CountryInformation>();
    }
}