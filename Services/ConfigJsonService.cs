using CountriesCapitalTelegramBot.Models;
using Newtonsoft.Json;
using static System.String;

namespace CountriesCapitalTelegramBot.Services;

public class ConfigJsonService : IConfigJsonService
{
    public ConfigJsonService(string filePath)
    {
        _filePath = filePath;
    }

    private readonly string _filePath;

    public string? GetTelegramBotApiFromConfigJson()
    {
        if (File.Exists(_filePath))
        {
            var jsonContent = File.ReadAllText(_filePath);
            
            var config = JsonConvert.DeserializeObject<Config>(jsonContent);

            return config?.TelegramBotApi;
        }
        
        Console.WriteLine("File wasn't found: " + _filePath);

        return Empty;
    }
    
    public string? GetAllCountriesApiFromConfigJson()
    {
        if (File.Exists(_filePath))
        {
            var jsonContent = File.ReadAllText(_filePath);
            
            var config = JsonConvert.DeserializeObject<Config>(jsonContent);

            return config?.AllCountriesInformationApi;
        }
        
        Console.WriteLine("File wasn't found: " + _filePath);

        return Empty;
    }
}