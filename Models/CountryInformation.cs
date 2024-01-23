using Newtonsoft.Json;

namespace CountriesCapitalTelegramBot.Models;

public class CountryInformation
{
    public CountryInformation(
        NameInfo nameInfo,
        List<string>? capital
        // string population, 
        // IList<string> languages, 
        // string flagImageUrl)
    )
    {
        Name = nameInfo;
        Capital = capital;
        // Population = population;
        // Languages = languages;
    }
    
    [JsonProperty("name")]
    public NameInfo? Name { get; set; }
    
    [JsonProperty("capital")]
    public List<string>? Capital { get; set; }
    
    // [JsonProperty("population")]
    // public string Population { get; set; }
    //
    // [JsonProperty("languages")]
    // public IList<string> Languages { get; set; }
}


public class NameInfo
{
    [JsonProperty("common")]
    public string Common { get; set; }

    [JsonProperty("official")]
    public string Official { get; set; }
}