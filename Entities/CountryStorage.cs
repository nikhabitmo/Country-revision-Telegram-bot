namespace CountriesCapitalTelegramBot.Entities;

public class CountryStorage
{
    private IList<CountryStorage> _countryStorage;

    public CountryStorage(IList<CountryStorage> countryStorage)
    {
        _countryStorage = countryStorage;
    }
}