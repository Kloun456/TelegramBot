using Telegram.Bot.Polling;
using Microsoft.Extensions.DependencyInjection;
using CoffeBot.Handlers;
using CoffeBot.Repositories;
using CoffeBot.Service;
using CoffeBot.Bot;
using CoffeBot.DataBase;
using System.Configuration;
using Microsoft.EntityFrameworkCore;

namespace TelegramBotExperiments
{
    class Program
    {
        private const string _errorBotStart = "Не получилось запустить бота";
        private static IServiceCollection ConfigureServices()
        {
            var services = new ServiceCollection();

            var connectionStringToDb = ConfigurationManager.ConnectionStrings["ConnectionToDb"].ConnectionString;
           
            services.AddScoped<Bot>();
            services.AddTransient<IUpdateHandler, UpdateHandler>();
            services.AddTransient<IRepositoryCup, RepositoryCup>();
            services.AddTransient<IMessageHandler, MessageHandler>();
            services.AddTransient<ICallbackQueryHandler, CallbackQueryHandler>();
            services.AddTransient<IBotService, BotService>();
            services.AddTransient<IRepositoryUser, RepositoryUser>();
            services.AddDbContext<ApplicationDbContext>(options => options.UseNpgsql(connectionStringToDb));

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
                Console.WriteLine(_errorBotStart);
            }
        }
    }
}