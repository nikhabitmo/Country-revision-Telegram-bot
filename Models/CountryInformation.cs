namespace CountriesCapitalTelegramBot.Models;

public class CountryInformation
{
    public CountryInformation(
        string commonName, 
        string officialName, 
        string capital, 
        string population, 
        IList<string> languages, 
        string flagImageUrl)
    {
        CommonName = commonName;
        OfficialName = officialName;
        Capital = capital;
        Population = population;
        Languages = languages;
        FlagImageUrl = flagImageUrl;
    }

    public string CommonName { get; set; }
    public string OfficialName { get; set; }
    public string Capital { get; set; }
    public string Population { get; set; }
    public IList<string> Languages { get; set; }
    public string FlagImageUrl { get; set; }
}