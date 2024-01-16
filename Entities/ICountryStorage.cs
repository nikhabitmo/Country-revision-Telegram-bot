using CountriesCapitalTelegramBot.Models;

namespace CountriesCapitalTelegramBot.Entities;

public interface ICountryStorage<T>
{
    T? FindCountryByCapital(string capital);
}