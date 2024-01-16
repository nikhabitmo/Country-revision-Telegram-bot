using System.Collections.ObjectModel;
using CountriesCapitalTelegramBot.Entities;
using CountriesCapitalTelegramBot.Models;
using CountriesCapitalTelegramBot.Services;
using RestSharp;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

var jsonService = new ConfigJsonService(@"..\..\..\Config.json");
ITelegramBotClient telegramBotClient =
    new TelegramBotClient(jsonService.GetTelegramBotApiFromConfigJson() ?? throw new NullReferenceException(nameof(jsonService)));

var countryStorage =
    new CountryStorage(await new ReceivingCountryInformationService(jsonService.GetAllCountriesApiFromConfigJson())
        .GetCountries());

using CancellationTokenSource cts = new ();
ReceiverOptions receiverOptions = new ()
{
    AllowedUpdates = Array.Empty<UpdateType>()
};

telegramBotClient.StartReceiving(
    updateHandler: HandleUpdateAsync,
    pollingErrorHandler: HandlePollingErrorAsync,
    receiverOptions: receiverOptions,
    cancellationToken: cts.Token
);

var me = await telegramBotClient.GetMeAsync();
Console.WriteLine($"Okay, I am user {me.Id} and this is {me.FirstName}.");

Console.WriteLine($"Start listening for @{me.Username}");
Console.ReadLine();

cts.Cancel();

async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
{
    if (update.Message is not { } message)
        return;
    if (message.Text is not { } messageText)
        return;

    var chatId = message.Chat.Id;

    Console.WriteLine($"Received a '{messageText}' message in chat {chatId}.");

    if (string.Equals(messageText, @"/country", StringComparison.Ordinal))
    {
        var sentMessageAsking = await botClient.SendTextMessageAsync(
            chatId: chatId,
            text: "Please say the capital of this country:\n" + messageText,
            cancellationToken: cancellationToken);

        var capitalInput = update.Message;
        Console.WriteLine(update.Message);

        var a = countryStorage.FindCountryByCapital("Moscow"); 
        sentMessageAsking = await botClient.SendTextMessageAsync(
            chatId: chatId,
            text: "Please say the capital of this country:\n" + messageText,
            cancellationToken: cancellationToken);
        
    }
}

Task HandlePollingErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
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