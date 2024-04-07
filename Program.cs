using Telegram.Bot.Exceptions;
using Telegram.Bot.Polling;
using Telegram.Bot.Types.Enums;
using Telegram.Bot;
using Microsoft.Extensions.DependencyInjection;
using CoffeBot.Handlers;
using CoffeBot.Repositories;
using CoffeBot.Service;

// https://t.me/lattelovecoffee

namespace TelegramBotExperiments
{
    class Program
    {
     
        private static ITelegramBotClient? _botClient;
        private static ReceiverOptions? _receiverOptions;

        private static IServiceCollection ConfigureServices()
        {
            var services = new ServiceCollection();
            services.AddScoped<UpdateHandler>();
            services.AddTransient<IRepositoryCup, RepositoryCup>();
            services.AddTransient<IBotService, BotService>();
            services.AddTransient<IRepositoryUser, RepositoryUser>();
            return services;
        }
            public static async Task Main()
        {
            
            _botClient = new TelegramBotClient("6435771255:AAHigtm3iMfduaIBEJMRkZqlwzlL5Ndcp8o"); // заменить получение токена из
            //appsetings.json

            _receiverOptions = new ReceiverOptions()
            {
                AllowedUpdates = [UpdateType.Message, UpdateType.CallbackQuery],
                ThrowPendingUpdates = true
            };

            using var cancellationToken = new CancellationTokenSource();
            
            var services = ConfigureServices();
            var serviceProvider = services.BuildServiceProvider();
            var updateHandler = serviceProvider.GetService<UpdateHandler>();

            _botClient?.StartReceiving(updateHandler.Update, ErrorHandler, _receiverOptions, cancellationToken.Token);

            var botUser = await _botClient.GetMeAsync();
            Console.WriteLine($"{botUser.FirstName} запущен!");

            await Task.Delay(-1);

        }
       
        private static Task ErrorHandler(ITelegramBotClient botClient, Exception error, CancellationToken cancellationToken)
        {
            var ErrorMessage = error switch
            {
                ApiRequestException apiRequestException
                    => $"Telegram API Error:\n[{apiRequestException.ErrorCode}]\n{apiRequestException.Message}",
                _ => error.ToString()
            };

            Console.WriteLine(ErrorMessage);
            return Task.CompletedTask;
        }
    }
}