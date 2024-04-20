using Telegram.Bot;
using Telegram.Bot.Types;

namespace CoffeBot.Handlers.Interface
{
    public interface IMessageHandler
    {
        Task HandleMessage(Update update, ITelegramBotClient botClient);
    }
}
