using Telegram.Bot;
using Telegram.Bot.Types;

namespace CoffeBot.Handlers.Interface
{
    public interface ICallbackQueryHandler
    {
        Task HandleCallbackQuery(Update update, ITelegramBotClient botClient);
    }
}
