using Newtonsoft.Json.Linq;
using RestSharp;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;

namespace CountriesCapitalTelegramBot;

public class Program
{
    static ITelegramBotClient bot = new TelegramBotClient("6831274348:AAFH1qActz-gFSlxxr2hkaVKu6kuTcjBFPw");
    private static Dictionary<long, string> userStates = new Dictionary<long, string>();

    private static async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update,
        CancellationToken cancellationToken)
    {
        Console.WriteLine(Newtonsoft.Json.JsonConvert.SerializeObject(update));

        if (update.Type == Telegram.Bot.Types.Enums.UpdateType.Message)
        {
            var message = update.Message;

            if (message.Text.ToLower() == "/start")
            {
                await botClient.SendTextMessageAsync(message.Chat, "Добро пожаловать на борт, добрый путник!");
                Message pollMessage = await botClient.SendPollAsync(
                    message.Chat,
                    question: "Did you ever hear the tragedy of Darth Plagueis The Wise?",
                    options: new[]
                    {
                        "Yes for the hundredth time!",
                        "No, who`s that?"
                    });

                return;
            }

            if (message.Text.ToLower() == "/country")
            {
                var result = GetRandomCountryAndCapital();
                string name = result.capital;
                string country = result.country;
                await botClient.SendTextMessageAsync(message.Chat, $"Назови столицу страны: {country}");
                botClient.StartReceiving(
                    updateHandler: update,
                    pollingErrorHandler: HandlePollingErrorAsync,
                    receiverOptions: new ReceiverOptions(),
                    cancellationToken: cancellationToken
                );
                if (message.Text == name)
                {
                    await botClient.SendTextMessageAsync(message.Chat, $"Все правильно, столица {country} это: {name}");
                }
                else
                {
                    await botClient.SendTextMessageAsync(message.Chat, $"Неверно, столица {country} это: {name}");
                }
                return;
            }

            await botClient.SendTextMessageAsync(message.Chat, "Привет-привет!!");
        }
    }
    


    private static Task HandleErrorAsync(ITelegramBotClient botClient, Exception exception,
        CancellationToken cancellationToken)
    {
        Console.WriteLine(Newtonsoft.Json.JsonConvert.SerializeObject(exception));
        return Task.CompletedTask;
    }

    private static (string country, string capital) GetRandomCountryAndCapital()
    {
        try
        {
            var client = new RestClient("https://restcountries.com/v2/all");
            var request = new RestRequest();
            request.Method = Method.Get;
            var response = client.Execute(request);

            if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                Console.WriteLine("Error: API Not Found");
                return ("Unknown", "Unknown");
            }

            dynamic jsonResponse = Newtonsoft.Json.JsonConvert.DeserializeObject(response.Content);

            if (jsonResponse is JArray jsonArray && jsonArray.Count > 0)
            {
                Random random = new Random();
                int randomIndex = random.Next(jsonArray.Count);

                string country = jsonArray[randomIndex]["name"].ToString();
                string capital = jsonArray[randomIndex]["capital"].ToString();

                return (country, capital);
            }

            Console.WriteLine("Error: Invalid JSON format");
            return ("Unknown", "Unknown");
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error: " + ex.Message);
            return ("Unknown", "Unknown");
        }
    }
    
    static Task HandlePollingErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
    {
        var ErrorMessage = exception switch
        {
            ApiRequestException apiRequestException
                => $"Telegram API Error:\n[{apiRequestException.ErrorCode}]\n{apiRequestException.Message}",
            _ => exception.ToString()
        };

        Console.WriteLine(ErrorMessage);
        return Task.CompletedTask;
    }

    static void Main(string[] args)
    {
        Console.WriteLine("Запущен бот " + bot.GetMeAsync().Result.FirstName);

        var cts = new CancellationTokenSource();
        var cancellationToken = cts.Token;
        var receiverOptions = new ReceiverOptions
        {
            AllowedUpdates = { },
        };

        bot.StartReceiving(
            HandleUpdateAsync,
            HandleErrorAsync,
            receiverOptions,
            cancellationToken
        );

        Console.ReadLine();
    }
}