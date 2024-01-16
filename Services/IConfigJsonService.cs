namespace CountriesCapitalTelegramBot.Services;

public interface IConfigJsonService
{
    public string? GetTelegramBotApiFromConfigJson();

    public string? GetAllCountriesApiFromConfigJson();
}