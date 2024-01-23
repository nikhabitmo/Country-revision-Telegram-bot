using CountriesCapitalTelegramBot.Models;

namespace CountriesCapitalTelegramBot.Entities;

public class CountryStorage : ICountryStorage<CountryInformation>
{
    private IList<CountryInformation> _countryStorage;

    public CountryStorage(IList<CountryInformation> countryStorage)
    {
        _countryStorage = countryStorage;
    }

    public CountryInformation? FindCountryByCapital(string capital)
    {
        return _countryStorage.FirstOrDefault(country =>
            string.Equals(country.Capital.FirstOrDefault(), capital, StringComparison.Ordinal));
    }

    public async Task<CountryInformation> GetRandomCountry()
    {
        var random = new Random();
        var randomIndex = random.Next(0, 196);
        return _countryStorage[randomIndex];
    }
}