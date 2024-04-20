using Telegram.Bot.Polling;
using Microsoft.Extensions.DependencyInjection;
using CoffeBot.Handlers;
using CoffeBot.Repositories;
using CoffeBot.Service;
using CoffeBot.Bot;
using CoffeBot.Repositories.Interfaces;
using CoffeBot.Service.Inerfaces;
using CoffeBot.Handlers.Interface;

namespace TelegramBotExperiments
{
    class Program
    {
        private static IServiceCollection ConfigureServices()
        {
            var services = new ServiceCollection();
            services.AddScoped<Bot>();

            services.AddTransient<IUpdateHandler, UpdateHandler>();
            services.AddTransient<IRepositoryCup, RepositoryCup>();
            services.AddTransient<IMessageHandler, MessageHandler>();
            services.AddTransient<ICallbackQueryHandler, CallbackQueryHandler>();
            services.AddTransient<IBotService, BotService>();
            services.AddTransient<IRepositoryUser, RepositoryUser>();
            return services;
        }

        public static async Task Main()
        {
            var services = ConfigureServices();
            var serviceProvider = services.BuildServiceProvider();
            var bot = serviceProvider.GetService<Bot>();
            if (bot is not null) 
            {
                await bot.Start();
            }
            else
            {
                Console.WriteLine("Не получилось запустить бота");
            }
        }
    }
}