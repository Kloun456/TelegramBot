using Telegram.Bot.Polling;
using Telegram.Bot;
using Telegram.Bot.Types.Enums;

namespace CoffeBot.Bot
{
    public class Bot
    {
        private static ITelegramBotClient? _botClient;
        private static ReceiverOptions? _receiverOptions;
        private static IUpdateHandler? _updateHandler;

        public Bot(IUpdateHandler updateHandler)
        {
            _updateHandler = updateHandler;

            _botClient = new TelegramBotClient("6435771255:AAHigtm3iMfduaIBEJMRkZqlwzlL5Ndcp8o"); 

            _receiverOptions = new ReceiverOptions()
            {
                AllowedUpdates = [UpdateType.Message, UpdateType.CallbackQuery],
                ThrowPendingUpdates = true
            };
        }
        public async Task Start()
        {
            using var cancellationToken = new CancellationTokenSource();

            _botClient?.StartReceiving(_updateHandler, _receiverOptions, cancellationToken.Token);

            var botUser = await _botClient.GetMeAsync();
            Console.WriteLine($"{botUser.FirstName} запущен!");

            await Task.Delay(-1);
        }
    }
}
