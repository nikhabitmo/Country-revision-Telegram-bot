namespace CountriesCapitalTelegramBot.Models;

public class CountryInformation
{
    public string Name { get; set; }
    public string Capital { get; set; }
    public string Population { get; set; }
    public IList<string> Languages { get; set; }
}